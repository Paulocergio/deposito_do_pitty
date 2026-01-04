using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace deposito_do_pitty.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsReceivableController : ControllerBase
    {
        private readonly IAccountsReceivableService _service;

        public AccountsReceivableController(IAccountsReceivableService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountsReceivableDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountsReceivableDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}
