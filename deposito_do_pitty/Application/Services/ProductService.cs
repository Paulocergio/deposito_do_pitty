using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;





public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Product?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<Product> CreateAsync(ProductDto dto)
    {
        if (await _repository.BarcodeExistsAsync(dto.Barcode))
            throw new Exception("Código de barras já existe.");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            PurchasePrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice,
            Category = dto.Category,
            StockQuantity = dto.StockQuantity,
            Status = dto.Status,
            Barcode = dto.Barcode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _repository.CreateAsync(product);
    }

    public async Task UpdateAsync(int id, ProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id) ?? throw new Exception("Produto não encontrado.");

        if (product.Barcode != dto.Barcode && await _repository.BarcodeExistsAsync(dto.Barcode))
            throw new Exception("Código de barras já existe.");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.PurchasePrice = dto.PurchasePrice;
        product.SalePrice = dto.SalePrice;
        product.Category = dto.Category;
        product.StockQuantity = dto.StockQuantity;
        product.Status = dto.Status;
        product.Barcode = dto.Barcode;
        product.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
}
