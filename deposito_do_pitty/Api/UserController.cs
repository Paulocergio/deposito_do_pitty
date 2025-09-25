using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.DTOs.Auth;
using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Application.Services;
using DepositoDoPitty.Application.DTOs;
using DepositoDoPitty.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DepositoDoPitty.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
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





        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            try
            {
                await _userService.CreateAsync(dto);
                return Created(string.Empty, dto); 
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID do corpo difere do ID da URL." });

            try
            {
                await _userService.UpdateAsync(dto);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }




        [HttpDelete("hard/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
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