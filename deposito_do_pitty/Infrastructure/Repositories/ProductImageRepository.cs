using deposito_do_pitty.Domain.Entities;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class ProductImageRepository : IProductImageRepository
{
    private readonly AppDbContext _context;

    public ProductImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductImage>> GetByProductIdAsync(int productId) =>
        await _context.ProductImages
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.IsPrimary)
            .ThenBy(x => x.SortOrder)
            .ToListAsync();

    public async Task<ProductImage?> GetByIdAsync(int id) =>
        await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddRangeAsync(IEnumerable<ProductImage> images)
    {
        await _context.ProductImages.AddRangeAsync(images);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductImage image)
    {
        _context.ProductImages.Remove(image);
        await _context.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
