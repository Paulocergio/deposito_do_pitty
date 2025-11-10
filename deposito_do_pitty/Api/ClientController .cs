using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace deposito_do_pitty.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost] 
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
          
            return Ok(clients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return client is null ? NotFound() : Ok(client);
        }

        [HttpPut("{documentNumber}")]
        public async Task<IActionResult> UpdateClient(string documentNumber, [FromBody] Client updatedClient)
        {
            if (documentNumber != updatedClient.DocumentNumber)
                return BadRequest("Documento não confere.");

            var client = await _clientService.GetByDocumentNumberAsync(documentNumber);
            if (client == null)
                return NotFound("Cliente não encontrado.");

            updatedClient.UpdatedAt = DateTime.UtcNow;
            await _clientService.UpdateAsync(updatedClient);
            return Ok("Cliente atualizado com sucesso.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound("Cliente não encontrado.");

            await _clientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
