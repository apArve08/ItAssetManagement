

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssetManagement.Core.DTOs;
using AssetManagement.Infrastructure.Services;

namespace AssetManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public DashboardController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            var data = await _assetService.GetDashboardDataAsync();
            return Ok(data);
        }
    }
}