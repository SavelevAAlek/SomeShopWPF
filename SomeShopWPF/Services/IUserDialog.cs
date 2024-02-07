namespace SomeShopWPF.Services
{
    public interface IUserDialog
    {
        void OpenAuthWindow();
        void OpenMainWindow();
        void OpenAddClientWindow();
        void OpenExtraWindow(string message);
    }
}
