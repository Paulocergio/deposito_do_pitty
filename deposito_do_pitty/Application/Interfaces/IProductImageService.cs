using deposito_do_pitty.Application.DTOs;
using Microsoft.AspNetCore.Http;

public interface IProductImageService
{
    Task<List<ProductImageDto>> UploadAsync(int productId, List<IFormFile> files);
    Task<List<ProductImageDto>> GetByProductIdAsync(int productId);
    Task DeleteAsync(int productId, int imageId);
    Task SetPrimaryAsync(int productId, int imageId);
}
