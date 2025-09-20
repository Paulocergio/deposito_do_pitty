using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Application.Interfaces
{
    public interface IClientService
    {
        Task ClientCreateAsync(Client client);
        Task<List<Client>> GetAllAsync();

    }
}