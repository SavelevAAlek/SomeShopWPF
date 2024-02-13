using SomeShopWPF.Commands;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class PurchasesViewModel : DialogViewModel
    {
        private string _productToBuy;
        private readonly Client _client;
        private readonly IRepository _repository;
        private ObservableCollection<Purchase> _purchases;

        public string ProductToBuy { get => _productToBuy; set => Set(ref _productToBuy, value); } 
        public List<string> ProductNames { get; set; }
        public ObservableCollection<Purchase> Purchases { get => _purchases; set => Set(ref _purchases, value); }

        #region Команда добавления покупки
        public ICommand AddPurchaseCommand { get; set; }
        private bool CanAddPurchaseCommandExecute() => true;
        private void OnAddPurchaseCommandExecuted(object? obj)
        {
            _repository.AddPurchase(_client, _productToBuy);
            _purchases = new ObservableCollection<Purchase>(_repository.GetPurchases(_client).Result);
            OnPropertyChanged(nameof(Purchases));
        }
        #endregion

        public PurchasesViewModel() { }
        public PurchasesViewModel(Client selectedClient, IRepository repository) : this()
        {
            _repository = repository;
            _client = selectedClient;
            ProductNames = _repository.GetProducts();
            _purchases = new ObservableCollection<Purchase>(_repository.GetPurchases(_client).Result);

            AddPurchaseCommand = new LambdaCommand(OnAddPurchaseCommandExecuted, CanAddPurchaseCommandExecute);
        }
    }
}
