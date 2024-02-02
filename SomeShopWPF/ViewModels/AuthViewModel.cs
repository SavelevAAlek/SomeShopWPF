using SomeShopWPF.ViewModels.Base;

namespace SomeShopWPF.ViewModels
{
    public class AuthViewModel : DialogViewModel
    {
        private string _username = "Логин";
        private string _password = "Пароль";

        public string Username { get => _username; set => Set(ref _username, value); }
        public string Password { get => _password; set => Set(ref _password, value); }


        public AuthViewModel()
        {

        }
    }
}
