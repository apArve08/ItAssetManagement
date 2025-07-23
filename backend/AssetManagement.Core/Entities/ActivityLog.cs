namespace AssetManagement.Core.Entities
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public Asset Asset { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}