using SomeShopWPF.Models;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Collections.Generic;

namespace SomeShopWPF.ViewModels
{
    public class PurchasesViewModel : DialogViewModel
    {
        private readonly Client _client;
        public IEnumerable<Purchase> Purchases { get => _client.Purchases; }
        public PurchasesViewModel(Client selectedClient) : this() => _client = selectedClient;

        public PurchasesViewModel() { }
    
    }
}
