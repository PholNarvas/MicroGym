using MicroGym.Service.RevenueSection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueSectionService     _revenueSectionService;
        private readonly ILogger<RevenueController> _logger;

        public RevenueController(
            IRevenueSectionService revenueSectionService,
            ILogger<RevenueController> logger)
        {
            _revenueSectionService = revenueSectionService;
            _logger                = logger;
        }

        [HttpGet("GetRevenue")]
        public async Task<IActionResult> GetRevenue(DateOnly month)
        {
            var revenue = await _revenueSectionService.GetRevenueService(month);
            return Ok(revenue);
        }

        [HttpGet("GetYearlyRevenue")]
        public async Task<IActionResult> GetYearlyRevenue()
        {
            var yearly = await _revenueSectionService.GetRevenueService();
            return Ok(yearly);
        }

        [HttpGet("GetRevenueChart")]
        public async Task<IActionResult> GetRevenueChart([FromQuery] int year)
        {
            var chart = await _revenueSectionService.GetRevenueChartByYearService(year);
            return Ok(chart);
        }
    }
}
