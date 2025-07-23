using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetManagement.Core.DTOs;
using AssetManagement.Infrastructure.Services;
using ClosedXML.Excel;

namespace AssetManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AssetDto>>> GetAssets([FromQuery] string? search = null)
        {
            var assets = await _assetService.GetAllAssetsAsync(search);
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetDto>> GetAsset(int id)
        {
            var asset = await _assetService.GetAssetByIdAsync(id);
            
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset);
        }

        [HttpPost]
        public async Task<ActionResult<AssetDto>> CreateAsset([FromBody] CreateAssetDto createDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var asset = await _assetService.CreateAssetAsync(createDto, userId);
            
            return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AssetDto>> UpdateAsset(int id, [FromBody] UpdateAssetDto updateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var asset = await _assetService.UpdateAssetAsync(id, updateDto, userId);
            
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _assetService.DeleteAssetAsync(id, userId);
            
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var assets = await _assetService.GetAllAssetsAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Assets");

            // Add headers
            worksheet.Cell(1, 1).Value = "Asset Tag";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Category";
            worksheet.Cell(1, 4).Value = "Status";
            worksheet.Cell(1, 5).Value = "Location";
            worksheet.Cell(1, 6).Value = "Assigned To";
            worksheet.Cell(1, 7).Value = "Serial Number";
            worksheet.Cell(1, 8).Value = "Model";
            worksheet.Cell(1, 9).Value = "Manufacturer";
            worksheet.Cell(1, 10).Value = "Purchase Date";
            worksheet.Cell(1, 11).Value = "Purchase Cost";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 11);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Add data
            for (int i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];
                var row = i + 2;
                
                worksheet.Cell(row, 1).Value = asset.AssetTag;
                worksheet.Cell(row, 2).Value = asset.Name;
                worksheet.Cell(row, 3).Value = asset.Category;
                worksheet.Cell(row, 4).Value = asset.Status;
                worksheet.Cell(row, 5).Value = asset.Location ?? "";
                worksheet.Cell(row, 6).Value = asset.AssignedToUserName ?? "";
                worksheet.Cell(row, 7).Value = asset.SerialNumber ?? "";
                worksheet.Cell(row, 8).Value = asset.Model ?? "";
                worksheet.Cell(row, 9).Value = asset.Manufacturer ?? "";
                worksheet.Cell(row, 10).Value = asset.PurchaseDate?.ToString("yyyy-MM-dd") ?? "";
                worksheet.Cell(row, 11).Value = asset.PurchaseCost?.ToString("C") ?? "";
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Save to memory stream
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Assets_{DateTime.Now:yyyyMMdd}.xlsx"
            );
        }
    }
}
