using SomeShopWPF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeShopWPF.Services
{
    public interface IRepository
    {
        IEnumerable<Client> GetClients();
        void Delete(Client selectedClient);
        Task<List<Purchase>> GetPurchases(Client selectedClient);
        void AddPurchase(Client selectedClient, string product);
        void AddClient(Client client);
        List<string> GetProducts();
        void EditClient(Client selectedClient);
    }
}
