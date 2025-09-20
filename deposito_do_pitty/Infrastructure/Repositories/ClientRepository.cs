using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deposito_do_pitty.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Client client)
        {
            await _context.Clients.AddAsync(client);    
            await _context.SaveChangesAsync();
        }

        public async Task<List<Client>> GetAllAsync()
        {
            //return await _context.Client.ToListAsync();
            return await _context.Clients.ToListAsync();
        
        }

        public async Task<Client?> GetByDocumentNumberAsync(string documentNumber)
        {
          return await _context.Clients
                           .FirstOrDefaultAsync(c => c.DocumentNumber == documentNumber);       
        }
    }
}


