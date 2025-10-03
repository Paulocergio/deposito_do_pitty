using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task AddAsync(Client client);
        Task<Client?> GetByIdAsync(int id);
        Task<Client?> GetByDocumentNumberAsync(string documentNumber);
        Task<List<Client>> GetAllAsync();
        Task UpdateAsync(Client client);
        Task DeleteAsync(int id);
    }
}
