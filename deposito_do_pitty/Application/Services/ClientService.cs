using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace deposito_do_pitty.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task ClientCreateAsync(Client client)
        {
            var existingDocument = await _clientRepository.GetByDocumentNumberAsync(client.DocumentNumber);
            if (existingDocument != null)
            {
                throw new InvalidOperationException("CNPJ já cadastrado");
            }

            var newClient = new Client
            {
                DocumentNumber = client.DocumentNumber,
                CompanyName = client.CompanyName,
                Address = client.Address,
                Phone = client.Phone,
                Email = client.Email,
                PostalCode = client.PostalCode,
                ContactPerson = client.ContactPerson,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _clientRepository.AddAsync(newClient);
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _clientRepository.GetAllAsync();
        }
    }
}
