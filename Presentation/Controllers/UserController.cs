using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Dto;


namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region User Management

        [HttpPost]
        [Authorize(Roles = "Admin")]  // فقط Admin
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var result = await _userService.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { userId = result.Data, message = "User created successfully" });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Advisor,Staff")]  // Admin, Advisor, Staff
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Advisor,Staff")]  // Admin, Advisor, Staff
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]  // فقط Admin
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "User deleted successfully" });
        }

        #endregion

        #region Client User Creation

        [HttpPost("create-client-user")]
        [Authorize(Roles = "Admin,Advisor")]  // Admin و Advisor
        public async Task<IActionResult> CreateClientUser([FromBody] CreateClientUserDto dto)
        {
            var result = await _userService.CreateClientUserAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { userId = result.Data, message = "Client user account created successfully" });
        }

        #endregion

        #region Household Management

        [HttpPost("household")]
        [Authorize(Roles = "Admin,Advisor")] 
        public async Task<IActionResult> CreateHousehold([FromBody] CreateHouseholdDto dto)
        {
            var result = await _userService.CreateHouseholdAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { householdId = result.Data, message = "Household created successfully" });
        }

        [HttpGet("households")]
        [Authorize(Roles = "Admin,Advisor,Staff")]  
        public async Task<IActionResult> GetHouseholds()
        {
            var result = await _userService.GetHouseholdsAsync();

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(result.Data);
        }

        #endregion
    }
}