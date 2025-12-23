using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace deposito_do_pitty.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            try
            {
                await _clientService.ClientCreateAsync(client);
                return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("update-by-document/{documentNumber}")]
        public async Task<IActionResult> UpdateClient(string documentNumber, [FromBody] Client updatedClient)
        {
            if (documentNumber != updatedClient.DocumentNumber) return BadRequest("Documento não confere.");
            var client = await _clientService.GetByDocumentNumberAsync(documentNumber);
            if (client == null) return NotFound("Cliente não encontrado.");
            updatedClient.UpdatedAt = DateTime.UtcNow;
            await _clientService.UpdateAsync(updatedClient);
            return Ok("Cliente atualizado com sucesso.");
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return client is null ? NotFound() : Ok(client);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound("Cliente não encontrado.");
            await _clientService.DeleteAsync(id);
            return NoContent();
        }
    }
}