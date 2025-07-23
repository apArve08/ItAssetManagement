namespace AssetManagement.Core.DTOs
{
    public class DashboardDto
    {
        public int TotalAssets { get; set; }
        public int AssignedAssets { get; set; }
        public int UnassignedAssets { get; set; }
        public int MaintenanceAssets { get; set; }
        public Dictionary<string, int> AssetsByCategory { get; set; } = new();
        public Dictionary<string, int> AssetsByStatus { get; set; } = new();
    }
}