using SomeShopWPF.Commands;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Data.SqlClient;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class AuthViewModel : DialogViewModel
    {
        private readonly IUserDialog _userDialog;

        private string _username = "Логин";
        private string _password = "Пароль";

        public string Username { get => _username; set => Set(ref _username, value); }
        public string Password { get => _password; set => Set(ref _password, value); }

        public ICommand LoginCommand { get; set; }

        public AuthViewModel(IUserDialog userDialog)
        {
            _userDialog = userDialog;
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
        }

        private void OnLoginCommandExecuted(object? obj)
        {
            _userDialog.OpenMainWindow();
            OnDialogComplete(EventArgs.Empty);
        }

        private bool CanLoginCommandExecute() => _password != "Пароль" ? true : false;
    }
}
