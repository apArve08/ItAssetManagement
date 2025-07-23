namespace AssetManagement.Core.DTOs
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchaseCost { get; set; }
        public int? AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public string? Location { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Manufacturer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    
    public class CreateAssetDto
    {
        public string AssetTag { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchaseCost { get; set; }
        public string? Location { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Manufacturer { get; set; }
    }
    
    public class UpdateAssetDto : CreateAssetDto
    {
        public int? AssignedToUserId { get; set; }
    }
}