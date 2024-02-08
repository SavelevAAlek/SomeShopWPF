using SomeShopWPF.Models;
using SomeShopWPF.ViewModels.Base;

namespace SomeShopWPF.Services
{
    public interface IUserDialog
    {
        void OpenAuthWindow();
        void OpenMainWindow();
        void OpenAddClientWindow();
        void OpenExtraWindow(string message);
        void OpenEditClientWindow(Client client);
    }
}
