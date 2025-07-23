
namespace AssetManagement.Core.Entities
{
    public class Asset
    {
        public int Id { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchaseCost { get; set; }
        public int? AssignedToUserId { get; set; }
        public string? Location { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedByUserId { get; set; }
        
        public User? AssignedToUser { get; set; }
        public User? CreatedByUser { get; set; }
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }
}