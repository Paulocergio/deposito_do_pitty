using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.DTOs.Auth;
using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Application.Services;
using DepositoDoPitty.Application.DTOs;
using DepositoDoPitty.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace DepositoDoPitty.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService , IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto dto)

        {
            try
            {
                await _userService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }



        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            await _userService.UpdateAsync(dto);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _userService.DeactivateAsync(id);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.AuthenticateAsync(request);

            if (result == null)
                return Unauthorized("Credenciais inválidas.");

            return Ok(result);
        }

    }
}