using SomeShopWPF.Commands;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class AddClientViewModel : DialogViewModel
    {
        private readonly IRepository _repository;
        private Client _newClient;

        public Client NewClient { get => _newClient; set => Set(ref _newClient, value); }

        #region Команда добавления нового клиента
        public ICommand AddClientCommand { get; set; }
        private bool CanAddClientCommandExecute()
        {
            if (_newClient.Surname != null &&
                _newClient.Name != null &&
                _newClient.Email != null &&
                _newClient.Patronymics != null) return true;

            else return false;
        }
        private void OnAddClientCommandExecuted(object? obj)
        {
            _repository.AddClient(_newClient);
            OnDialogComplete(EventArgs.Empty);
            _newClient = new Client();
        }
        #endregion

        public AddClientViewModel() { }
        public AddClientViewModel(IRepository repository) : this()
        {
            _repository = repository;
            _newClient = new Client();

            AddClientCommand = new LambdaCommand(OnAddClientCommandExecuted, CanAddClientCommandExecute);
        }    
    }
}
