using Microsoft.Extensions.DependencyInjection;
using SomeShopWPF.Views;
using System;

namespace SomeShopWPF.Services.Implementations
{
    internal class UserDialogService : IUserDialog
    {
        private readonly IServiceProvider _Services;

        public UserDialogService(IServiceProvider Services) => _Services = Services;

        private MainWindow? _mainWindow;
        public void OpenMainWindow()
        {
            if (_mainWindow is { } window)
            {
                window.Show();
                return;
            }

            window = _Services.GetRequiredService<MainWindow>();
            window.Closed += (_, _) => _mainWindow = null;

            _mainWindow = window;
            window.Show();
        }

        private AddClientWindow? _addClientWindow;
        public void OpenAddClientWindow()
        {
            if (_addClientWindow is { } window)
            {
                window.Show();
                return;
            }

            window = _Services.GetRequiredService<AddClientWindow>();
            window.Closed += (_, _) => _addClientWindow = null;

            _addClientWindow = window;
            window.Show();
        }

        private AuthWindow? _authWindow;
        public void OpenAuthWindow()
        {
            if (_authWindow is { } window)
            {
                window.Show();
                return;
            }

            window = _Services.GetRequiredService<AuthWindow>();
            window.Closed += (_, _) => _authWindow = null;

            _authWindow = window;
            window.Show();
        }
    }
}
