using SomeShopWPF.Models;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeShopWPF.ViewModels
{
    public class PurchasesViewModel : ViewModel
    {
        private readonly Client _client;

        public ObservableCollection<Purchase> Purchases { get; set; }
        public PurchasesViewModel(Client selectedClient) : this()
        {
            _client = selectedClient;
            Purchases = new ObservableCollection<Purchase>(_client.Purchases);
        }

        public PurchasesViewModel() { }
    
    }
}
