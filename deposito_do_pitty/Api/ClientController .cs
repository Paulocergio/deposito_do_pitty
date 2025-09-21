using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            try
            {
                await _clientService.ClientCreateAsync(client);
                return Ok(client);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); 
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clients = await _clientService.GetAllAsync();

                if (clients == null || !clients.Any())
                    return NotFound(new { message = "Nenhum cliente encontrado." });

                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro interno: {ex.Message}" });
            }
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

        [HttpDelete("{documentNumber}")]
        public async Task<IActionResult> DeleteClient(string documentNumber)
        {
            var client = await _clientService.GetByDocumentNumberAsync(documentNumber);

            if (client == null)
                return NotFound("Cliente não encontrado.");

            await _clientService.DeleteAsync(documentNumber);

            return Ok("Cliente excluído com sucesso.");
        }




    }
}

