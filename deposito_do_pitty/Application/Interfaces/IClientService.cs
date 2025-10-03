using deposito_do_pitty.Domain.Entities;

public interface IClientService
{
    Task ClientCreateAsync(Client client);             
    Task<List<Client>> GetAllAsync();           
    Task<Client?> GetByIdAsync(int id);            
    Task<Client?> GetByDocumentNumberAsync(string documentNumber); 
    Task UpdateAsync(Client client);               
    Task DeleteAsync(int id);                      
}
