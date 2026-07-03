using MicroGym.Service.RevenueSection;
using Microsoft.AspNetCore.Mvc;

namespace MicroGym.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueSectionService _revenueSectionService;
        public RevenueController(IRevenueSectionService revenueSectionService)
        {
            _revenueSectionService = revenueSectionService;
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

    }
}
