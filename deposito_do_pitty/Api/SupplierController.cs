using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace deposito_do_pitty.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid) return BadRequest(new { sucesso = false, mensagem = "Dados inválidos." });
            try
            {
                await _supplierService.AddSupplierAsync(supplier);
                return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, new { sucesso = true, mensagem = "Fornecedor criado com sucesso!", dados = supplier });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { sucesso = false, mensagem = ex.Message });
            }
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid) return BadRequest(new { sucesso = false, mensagem = "Dados inválidos." });
            var existing = await _supplierService.GetByIdAsync(id);
            if (existing == null) return NotFound(new { sucesso = false, mensagem = "Fornecedor não encontrado para atualização." });
            await _supplierService.UpdateSupplierAsync(id, supplier);
            return Ok(new { sucesso = true, mensagem = "Fornecedor atualizado com sucesso!" });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return Ok(new { sucesso = true, mensagem = "Fornecedores listados com sucesso!", dados = suppliers });
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            return supplier is null ? NotFound() : Ok(supplier);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _supplierService.GetByIdAsync(id);
            if (existing == null) return NotFound(new { sucesso = false, mensagem = "Fornecedor não encontrado para exclusão." });
            await _supplierService.DeleteSupplierAsync(id);
            return Ok(new { sucesso = true, mensagem = "Fornecedor excluído com sucesso!" });
        }
    }
}