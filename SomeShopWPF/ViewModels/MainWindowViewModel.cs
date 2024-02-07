using SomeShopWPF.Commands;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class MainWindowViewModel : DialogViewModel
    {
        private readonly IUserDialog _userDialog;
        private string _conStr = File.ReadAllText(@"..\..\..\Resources\MSSQLcon_str.txt");

        private Client _selectedClient;

        private ViewModel _extraView;

        public ViewModel ExtraView { get => _extraView; set => Set(ref _extraView, value); }

        public ObservableCollection<Client> ClientsList { get; set; } = new ObservableCollection<Client>();
        public Client SelectedClient { get => _selectedClient; set => Set(ref _selectedClient, value); }

        public ICommand DeleteCommand { get; set; }
        public ICommand OpenAddWindowCommand { get; set; }

        public MainWindowViewModel(IUserDialog userDialog)
        {
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            OpenAddWindowCommand = new LambdaCommand(OnOpenAddWindowCommandExecuted, CanOpenAddWindowCommandExecute);
            SetClientTable();
            _userDialog = userDialog;
        }

        private bool CanOpenAddWindowCommandExecute() => true;

        private void OnOpenAddWindowCommandExecuted(object? obj) => _userDialog.OpenAddClientWindow();

        private bool CanDeleteCommandExecute() => SelectedClient != null ? true : false;

        private void OnDeleteCommandExecuted(object? obj)
        {
            DeleteClient(SelectedClient);
            OnPropertyChanged(nameof(ClientsList));
        }

        private void SetClientTable()
        {
            string query = "SELECT * FROM Clients";

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ClientsList.Add(new Client(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5)));
                }
            }

        }

        private void DeleteClient(Client selectedClient)
        {
            var query = $"DELETE FROM Clients WHERE Email = '{selectedClient.Email}'";

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();
            }

            ClientsList = new ObservableCollection<Client>();
            SetClientTable();
        }
    }
}
