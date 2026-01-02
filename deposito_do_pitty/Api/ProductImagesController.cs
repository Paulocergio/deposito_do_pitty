using deposito_do_pitty.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace deposito_do_pitty.Api
{
    [ApiController]
    [Route("api/products/{productId:int}/images")]
    [Authorize]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _service;

        public ProductImagesController(IProductImageService service)
        {
            _service = service;
        }

        [HttpPost]
        [RequestSizeLimit(30_000_000)] // 30MB total (ajuste)
        public async Task<IActionResult> Upload(int productId, [FromForm] List<IFormFile> files)
        {
            var result = await _service.UploadAsync(productId, files);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int productId)
        {
            var list = await _service.GetByProductIdAsync(productId);
            return Ok(list);
        }

        [HttpDelete("{imageId:int}")]
        public async Task<IActionResult> Delete(int productId, int imageId)
        {
            await _service.DeleteAsync(productId, imageId);
            return NoContent();
        }

        [HttpPut("{imageId:int}/primary")]
        public async Task<IActionResult> SetPrimary(int productId, int imageId)
        {
            await _service.SetPrimaryAsync(productId, imageId);
            return NoContent();
        }
    }
}
