using Microsoft.Extensions.DependencyInjection;
using SomeShopWPF.Models;
using SomeShopWPF.ViewModels;
using SomeShopWPF.Views;
using System;
using System.Windows;

namespace SomeShopWPF.Services.Implementations
{
    internal class UserDialogService : IUserDialog
    {
        private readonly IServiceProvider _Services;
        private MainWindow? _mainWindow;
        private AddClientWindow? _addClientWindow;
        private AuthWindow? _authWindow;
        private EditeClientWindow? _editClientWindow;

        public UserDialogService(IServiceProvider Services) => _Services = Services;

        /// <summary>
        /// Открытие главного окна
        /// </summary>
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

        /// <summary>
        /// Открытие окна с добавлением клиента
        /// </summary>
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

        /// <summary>
        /// Открытие окна авторизации
        /// </summary>
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

        /// <summary>
        /// Открытие окна с сообщениями о выполнении команды
        /// </summary>
        /// <param name="message"></param>
        public void OpenExtraWindow(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Открытие окна редактирования клиента
        /// </summary>
        /// <param name="selectedClient"></param>
        public void OpenEditClientWindow(Client selectedClient)
        {
            var s = _Services.GetRequiredService<IRepository>();
            if (_editClientWindow is { } window)
            {               
                window.DataContext = new EditClientViewModel(selectedClient, s);
                window.Show();
                return;
            }

            window = _Services.GetRequiredService<EditeClientWindow>();
            window.Closed += (_, _) => _editClientWindow = null;

            _editClientWindow = window;
            _editClientWindow.DataContext = new EditClientViewModel(selectedClient, s);
            window.Show();
        }
    }
}
