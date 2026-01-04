using deposito_do_pitty.Application.DTOs;


namespace deposito_do_pitty.Application.Interfaces
{
    public interface ISalesService
    {
        Task<List<SaleDto>> GetAllAsync();
        Task<SaleDto?> GetByIdAsync(int id);

        Task<int> CreateAsync(CreateSaleDto dto);

        Task CancelAsync(int id);
        Task DeleteAsync(int id);
    }
}
