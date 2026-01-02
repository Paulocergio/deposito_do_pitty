using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class ProductImageService : IProductImageService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductImageRepository _imageRepository;
    private readonly IWebHostEnvironment _env;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    { ".jpg", ".jpeg", ".png", ".webp" };

    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB por foto (ajuste se quiser)

    public ProductImageService(
        IProductRepository productRepository,
        IProductImageRepository imageRepository,
        IWebHostEnvironment env)
    {
        _productRepository = productRepository;
        _imageRepository = imageRepository;
        _env = env;
    }

    public async Task<List<ProductImageDto>> UploadAsync(int productId, List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            throw new Exception("Nenhuma imagem enviada.");

        var product = await _productRepository.GetByIdAsync(productId);
        if (product is null)
            throw new Exception("Produto não encontrado.");

        var webRoot = _env.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
            webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var folder = Path.Combine(webRoot, "uploads", "products", productId.ToString());
        Directory.CreateDirectory(folder);

        var existing = await _imageRepository.GetByProductIdAsync(productId);
        var nextSort = existing.Count == 0 ? 0 : existing.Max(x => x.SortOrder) + 1;

        var entities = new List<ProductImage>();

        foreach (var file in files)
        {
            if (file.Length <= 0) continue;
            if (file.Length > MaxFileSizeBytes)
                throw new Exception($"Arquivo muito grande: {file.FileName}. Máximo: {MaxFileSizeBytes / (1024 * 1024)}MB.");

            var ext = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(ext))
                throw new Exception($"Extensão não permitida: {ext}. Use jpg, jpeg, png, webp.");

            if (string.IsNullOrWhiteSpace(file.ContentType) || !file.ContentType.StartsWith("image/"))
                throw new Exception($"Content-Type inválido: {file.ContentType}.");

            var safeName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(folder, safeName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);

            var url = $"/uploads/products/{productId}/{safeName}";

            entities.Add(new ProductImage
            {
                ProductId = productId,
                FileName = safeName,
                ContentType = file.ContentType,
                Size = file.Length,
                Url = url,
                SortOrder = nextSort++,
                IsPrimary = existing.Count == 0 && entities.Count == 0 // primeira foto do produto vira principal
            });
        }

        if (entities.Count == 0)
            throw new Exception("Nenhuma imagem válida foi enviada.");

        await _imageRepository.AddRangeAsync(entities);

        return entities.Select(ToDto).ToList();
    }

    public async Task<List<ProductImageDto>> GetByProductIdAsync(int productId)
    {
        var images = await _imageRepository.GetByProductIdAsync(productId);
        return images.Select(ToDto).ToList();
    }

    public async Task DeleteAsync(int productId, int imageId)
    {
        var img = await _imageRepository.GetByIdAsync(imageId);
        if (img is null || img.ProductId != productId)
            throw new Exception("Imagem não encontrada.");

        var webRoot = _env.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
            webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var fullPath = Path.Combine(webRoot, "uploads", "products", productId.ToString(), img.FileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        var wasPrimary = img.IsPrimary;

        await _imageRepository.DeleteAsync(img);

        if (wasPrimary)
        {
            var remaining = await _imageRepository.GetByProductIdAsync(productId);
            var first = remaining.OrderBy(x => x.SortOrder).FirstOrDefault();
            if (first != null)
            {
                first.IsPrimary = true;
                await _imageRepository.SaveChangesAsync();
            }
        }
    }

    public async Task SetPrimaryAsync(int productId, int imageId)
    {
        var images = await _imageRepository.GetByProductIdAsync(productId);
        if (images.Count == 0)
            throw new Exception("Produto não possui imagens.");

        var target = images.FirstOrDefault(x => x.Id == imageId);
        if (target is null)
            throw new Exception("Imagem não encontrada.");

        foreach (var img in images)
            img.IsPrimary = img.Id == imageId;

        await _imageRepository.SaveChangesAsync();
    }

    private static ProductImageDto ToDto(ProductImage x) => new()
    {
        Id = x.Id,
        ProductId = x.ProductId,
        Url = x.Url,
        IsPrimary = x.IsPrimary,
        SortOrder = x.SortOrder
    };
}
