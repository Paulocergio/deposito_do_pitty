using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace deposito_do_pitty.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _supplierService.GetAllAsync();

            if (!suppliers.Any())
                return NotFound(new { sucesso = false, mensagem = "Nenhum fornecedor encontrado." });

            return Ok(new
            {
                sucesso = true,
                mensagem = "Fornecedores listados com sucesso!",
                dados = suppliers
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { sucesso = false, mensagem = "Dados inválidos." });

            await _supplierService.AddSupplierAsync(supplier);

            return Ok(new
            {
                sucesso = true,
                mensagem = "Fornecedor criado com sucesso!",
                dados = supplier
            });
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] Supplier supplier)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { sucesso = false, mensagem = "Dados inválidos." });

            var existing = await _supplierService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { sucesso = false, mensagem = "Fornecedor não encontrado para atualização." });

            await _supplierService.UpdateSupplierAsync(id, supplier);

            return Ok(new { sucesso = true, mensagem = "Fornecedor atualizado com sucesso!" });
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _supplierService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { sucesso = false, mensagem = "Fornecedor não encontrado para exclusão." });

            await _supplierService.DeleteSupplierAsync(id);
            return Ok(new { sucesso = true, mensagem = "Fornecedor excluído com sucesso!" });
        }
    }
}
