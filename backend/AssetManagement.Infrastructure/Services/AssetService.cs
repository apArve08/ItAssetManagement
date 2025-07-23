using Microsoft.EntityFrameworkCore;
using AssetManagement.Core.DTOs;
using AssetManagement.Core.Entities;
using AssetManagement.Infrastructure.Data;

namespace AssetManagement.Infrastructure.Services
{
    public interface IAssetService
    {
        Task<List<AssetDto>> GetAllAssetsAsync(string? searchTerm = null);
        Task<AssetDto?> GetAssetByIdAsync(int id);
        Task<AssetDto> CreateAssetAsync(CreateAssetDto createDto, int userId);
        Task<AssetDto?> UpdateAssetAsync(int id, UpdateAssetDto updateDto, int userId);
        Task<bool> DeleteAssetAsync(int id, int userId);
        Task<DashboardDto> GetDashboardDataAsync();
    }

    public class AssetService : IAssetService
    {
        private readonly ApplicationDbContext _context;

        public AssetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssetDto>> GetAllAssetsAsync(string? searchTerm = null)
        {
            var query = _context.Assets
                .Include(a => a.AssignedToUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(a => 
                    a.AssetTag.ToLower().Contains(searchTerm) ||
                    a.Name.ToLower().Contains(searchTerm) ||
                    (a.SerialNumber != null && a.SerialNumber.ToLower().Contains(searchTerm)) ||
                    (a.Location != null && a.Location.ToLower().Contains(searchTerm))
                );
            }

            var assets = await query
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => MapToDto(a))
                .ToListAsync();

            return assets;
        }

        public async Task<AssetDto?> GetAssetByIdAsync(int id)
        {
            var asset = await _context.Assets
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(a => a.Id == id);

            return asset == null ? null : MapToDto(asset);
        }

        public async Task<AssetDto> CreateAssetAsync(CreateAssetDto createDto, int userId)
        {
            var asset = new Asset
            {
                AssetTag = createDto.AssetTag,
                Name = createDto.Name,
                Description = createDto.Description,
                Category = createDto.Category,
                Status = createDto.Status,
                PurchaseDate = createDto.PurchaseDate,
                PurchaseCost = createDto.PurchaseCost,
                Location = createDto.Location,
                SerialNumber = createDto.SerialNumber,
                Model = createDto.Model,
                Manufacturer = createDto.Manufacturer,
                CreatedByUserId = userId
            };

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            // Log activity
            await LogActivityAsync(asset.Id, userId, "Created", $"Asset {asset.AssetTag} created");

            return MapToDto(asset);
        }

        public async Task<AssetDto?> UpdateAssetAsync(int id, UpdateAssetDto updateDto, int userId)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return null;

            var oldStatus = asset.Status;
            var oldAssignedUserId = asset.AssignedToUserId;

            asset.AssetTag = updateDto.AssetTag;
            asset.Name = updateDto.Name;
            asset.Description = updateDto.Description;
            asset.Category = updateDto.Category;
            asset.Status = updateDto.Status;
            asset.PurchaseDate = updateDto.PurchaseDate;
            asset.PurchaseCost = updateDto.PurchaseCost;
            asset.Location = updateDto.Location;
            asset.SerialNumber = updateDto.SerialNumber;
            asset.Model = updateDto.Model;
            asset.Manufacturer = updateDto.Manufacturer;
            asset.AssignedToUserId = updateDto.AssignedToUserId;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Log activity
            var description = $"Asset {asset.AssetTag} updated";
            if (oldAssignedUserId != asset.AssignedToUserId)
            {
                if (asset.AssignedToUserId.HasValue)
                {
                    description = $"Asset {asset.AssetTag} assigned to user ID {asset.AssignedToUserId}";
                    await LogActivityAsync(asset.Id, userId, "Assigned", description);
                }
                else
                {
                    description = $"Asset {asset.AssetTag} unassigned";
                    await LogActivityAsync(asset.Id, userId, "Unassigned", description);
                }
            }
            else
            {
                await LogActivityAsync(asset.Id, userId, "Updated", description);
            }

            await _context.Entry(asset).Reference(a => a.AssignedToUser).LoadAsync();
            return MapToDto(asset);
        }

        public async Task<bool> DeleteAssetAsync(int id, int userId)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null) return false;

            await LogActivityAsync(asset.Id, userId, "Deleted", $"Asset {asset.AssetTag} deleted");

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var assets = await _context.Assets.ToListAsync();

            var dashboard = new DashboardDto
            {
                TotalAssets = assets.Count,
                AssignedAssets = assets.Count(a => a.AssignedToUserId.HasValue),
                UnassignedAssets = assets.Count(a => !a.AssignedToUserId.HasValue),
                MaintenanceAssets = assets.Count(a => a.Status == "Maintenance"),
                AssetsByCategory = assets.GroupBy(a => a.Category)
                    .ToDictionary(g => g.Key, g => g.Count()),
                AssetsByStatus = assets.GroupBy(a => a.Status)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return dashboard;
        }

        private async Task LogActivityAsync(int assetId, int userId, string action, string description)
        {
            var log = new ActivityLog
            {
                AssetId = assetId,
                UserId = userId,
                Action = action,
                Description = description
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        private static AssetDto MapToDto(Asset asset)
        {
            return new AssetDto
            {
                Id = asset.Id,
                AssetTag = asset.AssetTag,
                Name = asset.Name,
                Description = asset.Description,
                Category = asset.Category,
                Status = asset.Status,
                PurchaseDate = asset.PurchaseDate,
                PurchaseCost = asset.PurchaseCost,
                AssignedToUserId = asset.AssignedToUserId,
                AssignedToUserName = asset.AssignedToUser?.FullName,
                Location = asset.Location,
                SerialNumber = asset.SerialNumber,
                Model = asset.Model,
                Manufacturer = asset.Manufacturer,
                CreatedAt = asset.CreatedAt,
                UpdatedAt = asset.UpdatedAt
            };
        }
    }
}