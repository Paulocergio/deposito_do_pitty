using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task AddAsync(Client client);
        Task<Client?> GetByDocumentNumberAsync(string documentNumber);
    }
}
