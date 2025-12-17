using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace deposito_do_pitty.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsPayableController : ControllerBase
    {
        private readonly IAccountsPayableService _service;

        public AccountsPayableController(IAccountsPayableService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountsPayableDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountsPayableDto dto)
        {
            if (dto == null) return BadRequest("Dados inválidos.");
            await _service.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var all = await _service.GetAllAsync();
            var one = all.FirstOrDefault(x => x.Id == id);
            return one is null ? NotFound() : Ok(one);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}