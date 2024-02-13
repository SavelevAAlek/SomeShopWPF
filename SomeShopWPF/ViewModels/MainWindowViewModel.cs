using SomeShopWPF.Commands;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class MainWindowViewModel : DialogViewModel
    {
        private readonly IRepository _repository;
        private readonly IUserDialog _userDialog;
        private Client _selectedClient;
        private ObservableCollection<Client> _clientsList;
        private ViewModel _extraView;

        public Client SelectedClient { get => _selectedClient; set => Set(ref _selectedClient, value); }
        public ObservableCollection<Client> ClientsList { get => _clientsList; set => Set(ref _clientsList, value); }
        public ViewModel ExtraView { get => _extraView; set => Set(ref _extraView, value); }

        #region Команда удаления клиента
        public ICommand DeleteCommand { get; set; }
        private bool CanDeleteCommandExecute() => SelectedClient != null ? true : false;
        private void OnDeleteCommandExecuted(object? obj)
        {
            _repository.Delete(SelectedClient);
            _clientsList = new ObservableCollection<Client>(_repository.GetClients());
            OnPropertyChanged(nameof(ClientsList));
        }
        #endregion

        #region Команда открытия окна добавления клиента
        public ICommand OpenAddWindowCommand { get; set; }
        private bool CanOpenAddWindowCommandExecute() => true;
        private void OnOpenAddWindowCommandExecuted(object? obj) => _userDialog.OpenAddClientWindow();
        #endregion

        #region Команда открытия окна редактирования клиента
        public ICommand OpenEditClientCommand { get; set; }
        private bool CanOpenEditClientCommandExecute() => _selectedClient != null ? true : false;
        private void OnOpenEditClientCommandExecuted(object? obj) => _userDialog.OpenEditClientWindow(SelectedClient);
        #endregion

        #region Команда открытия списка покупок клиента
        public ICommand ShowPurchasesCommand { get; set; }
        private bool CanShowPurchasesCommandExecute() => _selectedClient != null ? true : false;
        private void OnShowPurchasesCommandExecuted(object? obj) => ExtraView = new PurchasesViewModel(_selectedClient, _repository);
        #endregion

        public MainWindowViewModel() { }
        public MainWindowViewModel(IUserDialog userDialog, IRepository repository) : this()
        {
            _userDialog = userDialog;
            _repository = repository;
            _clientsList = new ObservableCollection<Client>(_repository.GetClients());

            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            OpenAddWindowCommand = new LambdaCommand(OnOpenAddWindowCommandExecuted, CanOpenAddWindowCommandExecute);
            ShowPurchasesCommand = new LambdaCommand(OnShowPurchasesCommandExecuted, CanShowPurchasesCommandExecute);
            OpenEditClientCommand = new LambdaCommand(OnOpenEditClientCommandExecuted, CanOpenEditClientCommandExecute);
        }
    }
}
