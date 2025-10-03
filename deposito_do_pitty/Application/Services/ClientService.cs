using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;

namespace deposito_do_pitty.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task ClientCreateAsync(Client client)
        {
            var existing = await _repository.GetByDocumentNumberAsync(client.DocumentNumber);
            if (existing != null)
            {
                throw new InvalidOperationException("Cliente já está cadastrado.");
            }

            await _repository.AddAsync(client);
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Client?> GetByDocumentNumberAsync(string documentNumber)
        {
            return await _repository.GetByDocumentNumberAsync(documentNumber);
        }

        public async Task UpdateAsync(Client client)
        {
            await _repository.UpdateAsync(client);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
