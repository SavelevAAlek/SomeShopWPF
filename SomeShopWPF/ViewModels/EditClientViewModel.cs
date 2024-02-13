using SomeShopWPF.Commands;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class EditClientViewModel : DialogViewModel
    {
        private readonly IRepository _repository;
        private Client _selectedClient;

        public Client SelectedClient { get => _selectedClient; set => Set(ref _selectedClient, value); }

        #region Команда редактирования клиента
        public ICommand EditClientCommand { get; set; }
        private bool CanEditClientCommandExecute()
        {
            if (_selectedClient.Surname != null &&
                _selectedClient.Name != null &&
                _selectedClient.Email != null &&
                _selectedClient.Patronymics != null) return true;

            else return false;
        }
        private void OnEditClientCommandExecuted(object? obj)
        {
            _repository.EditClient(_selectedClient);
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion

        public EditClientViewModel() { }
        public EditClientViewModel(Client selectedClient, IRepository repository) : this()
        {
            _repository = repository;
            _selectedClient = new Client(selectedClient);

            EditClientCommand = new LambdaCommand(OnEditClientCommandExecuted, CanEditClientCommandExecute);
        }      
    }
}
