using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Application.Common.Dto;
    using Domain.Abstractions;
    using System.Threading.Tasks;

    namespace Presentation.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        [Authorize]
        public class DashboardController : ControllerBase
        {
            private readonly IDashboardService _dashboardService;

            public DashboardController(IDashboardService dashboardService)
            {
                _dashboardService = dashboardService;
            }

            [HttpGet]
            [Authorize(Roles = "Admin,Advisor,Staff")] 
            public async Task<IActionResult> GetDashboard()
            {
                var result = await _dashboardService.GetDashboardAsync();

                if (!result.IsSuccess)
                    return BadRequest(new { error = result.Message });

                return Ok(result.Data);
            }
        }
    }
}
