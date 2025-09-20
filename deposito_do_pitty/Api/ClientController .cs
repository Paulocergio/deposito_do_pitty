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
    }
}

