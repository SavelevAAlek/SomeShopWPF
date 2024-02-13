using SomeShopWPF.Commands;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class AuthViewModel : DialogViewModel
    {
        private readonly IUserDialog _userDialog;
        private string _username;
        private string _password;

        public string Username { get => _username; set => Set(ref _username, value); }
        public string Password { get => _password; set => Set(ref _password, value); }

        #region Команда авторизации (пока заглушка)
        public ICommand LoginCommand { get; set; }
        private bool CanLoginCommandExecute() => _password != null ? true : false;
        private void OnLoginCommandExecuted(object? obj)
        {
            _userDialog.OpenMainWindow();
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion

        public AuthViewModel() { }
        public AuthViewModel(IUserDialog userDialog) : this()
        {
            _userDialog = userDialog;
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
        }
    }
}
