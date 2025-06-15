using CashflowAI.Domain.DTOs;
using CashflowAI.Domain.Entities;
using CashflowAI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashflowAI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserCreateDto registerDto)
        {
            var result = await _userService.CreateUserAsync(registerDto);
            return Ok(result);
        }

        //[HttpPost("login")]
        //public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        //{
        //    //var user = await _userService.LoginAsync(loginDto.Username, loginDto.Password);
        //    //if (user == null) return Unauthorized();

        //    //return Ok(MapToDto(user));
        //    return Ok();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto userDto)
        {
            await _userService.UpdateAsync(id, userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
} 