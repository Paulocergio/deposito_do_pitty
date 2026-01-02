using deposito_do_pitty.Domain.Entities;

public interface IProductImageRepository
{
    Task<List<ProductImage>> GetByProductIdAsync(int productId);
    Task<ProductImage?> GetByIdAsync(int id);
    Task AddRangeAsync(IEnumerable<ProductImage> images);
    Task DeleteAsync(ProductImage image);
    Task SaveChangesAsync();
}
