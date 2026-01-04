using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Enums;
using deposito_do_pitty.Domain.Interfaces;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deposito_do_pitty.Application.Services
{
    public class SalesService : ISalesService
    {
        private readonly AppDbContext _context;
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        public SalesService(
            AppDbContext context,
            ISaleRepository saleRepository,
            IProductRepository productRepository
        )
        {
            _context = context;
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public async Task<int> CreateAsync(CreateSaleDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                throw new Exception("A venda precisa ter ao menos 1 item.");

            var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != productIds.Count)
                throw new Exception("Um ou mais produtos informados não existem.");

            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1) Atualiza estoque
                foreach (var item in dto.Items)
                {
                    var prod = products.First(p => p.Id == item.ProductId);
                    var stock = prod.StockQuantity;

                    if (stock <= 0)
                        throw new Exception($"Estoque zerado para o produto: {prod.Name}.");

                    if (item.Quantity <= 0)
                        throw new Exception($"Quantidade inválida para o produto: {prod.Name}.");

                    if (item.Quantity > stock)
                        throw new Exception($"Quantidade maior que o estoque ({stock}) para o produto: {prod.Name}.");

                    prod.StockQuantity = stock - item.Quantity;

                    // Se seu Product não tiver UpdatedAt, remova esta linha.
                    prod.UpdatedAt = DateTime.UtcNow;
                }

                // 2) Calcula totais
                var subtotal = dto.Items.Sum(i => i.UnitPrice * i.Quantity);

                var discountPercent = dto.DiscountPercent < 0 ? 0 : dto.DiscountPercent;
                if (discountPercent > 100) discountPercent = 100;

                var discountValue = subtotal * (discountPercent / 100m);
                var total = subtotal - discountValue;
                if (total < 0) total = 0;

                var now = DateTimeOffset.UtcNow;

                // 3) Monta entidade Sale (paymenttype/status são TEXT no banco)
                var sale = new Sale
                {
                    customerid = dto.CustomerId,
                    customername = dto.CustomerName,
                    date = dto.Date == default ? now : ToDtoOffset(dto.Date),

                    paymenttype = dto.PaymentType.ToString(),
                    status = SaleStatus.CONCLUIDA.ToString(),

                    subtotal = subtotal,
                    discountpercent = discountPercent,
                    discountvalue = discountValue,
                    total = total,

                    createdat = now,
                    updatedat = now,

                    items = dto.Items.Select(i => new SaleItem
                    {
                        productid = i.ProductId,
                        quantity = i.Quantity,
                        unitprice = i.UnitPrice,
                        subtotal = i.UnitPrice * i.Quantity,
                        createdat = now
                    }).ToList()
                };

                await _saleRepository.AddAsync(sale);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return sale.id;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<List<SaleDto>> GetAllAsync()
        {
            var list = await _saleRepository.GetAllAsync();
            return list.Select(MapToDto).ToList();
        }

        public async Task<SaleDto?> GetByIdAsync(int id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            return sale == null ? null : MapToDto(sale);
        }

        public async Task CancelAsync(int id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null) throw new Exception("Venda não encontrada.");

            if (StringToSaleStatus(sale.status) == SaleStatus.CANCELADA)
                return;

            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var productIds = sale.items.Select(i => i.productid).Distinct().ToList();

                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var it in sale.items)
                {
                    var prod = products.First(p => p.Id == it.productid);
                    prod.StockQuantity += it.quantity;

                    // Se seu Product não tiver UpdatedAt, remova esta linha.
                    prod.UpdatedAt = DateTime.UtcNow;
                }

                sale.status = SaleStatus.CANCELADA.ToString();
                sale.updatedat = DateTimeOffset.UtcNow;

                await _saleRepository.UpdateAsync(sale);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null) throw new Exception("Venda não encontrada.");

            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // Se não estiver cancelada, devolve estoque antes de deletar
                if (StringToSaleStatus(sale.status) != SaleStatus.CANCELADA)
                {
                    var productIds = sale.items.Select(i => i.productid).Distinct().ToList();

                    var products = await _context.Products
                        .Where(p => productIds.Contains(p.Id))
                        .ToListAsync();

                    foreach (var it in sale.items)
                    {
                        var prod = products.First(p => p.Id == it.productid);
                        prod.StockQuantity += it.quantity;

                        // Se seu Product não tiver UpdatedAt, remova esta linha.
                        prod.UpdatedAt = DateTime.UtcNow;
                    }
                }

                await _saleRepository.DeleteAsync(sale);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        private static SaleDto MapToDto(Sale s) => new SaleDto
        {
            Id = s.id,
            CustomerId = s.customerid,
            CustomerName = s.customername,
            Date = s.date.UtcDateTime,
            PaymentType = StringToPaymentType(s.paymenttype),
            Status = StringToSaleStatus(s.status),
            Subtotal = s.subtotal,
            DiscountPercent = s.discountpercent,
            DiscountValue = s.discountvalue,
            Total = s.total,
            Items = s.items.Select(i => new SaleItemDto
            {
                Id = i.id,
                ProductId = i.productid,
                Quantity = i.quantity,
                UnitPrice = i.unitprice,
                Subtotal = i.subtotal
            }).ToList(),
            CreatedAt = s.createdat.UtcDateTime,
            UpdatedAt = s.updatedat.UtcDateTime
        };

        private static DateTimeOffset ToDtoOffset(DateTime dt)
        {
            var utc = dt.Kind == DateTimeKind.Utc ? dt : DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            return new DateTimeOffset(utc);
        }

        private static PaymentType StringToPaymentType(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;

            return Enum.TryParse<PaymentType>(value, true, out var parsed)
                ? parsed
                : default;
        }

        private static SaleStatus StringToSaleStatus(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;

            return Enum.TryParse<SaleStatus>(value, true, out var parsed)
                ? parsed
                : default;
        }
    }
}
