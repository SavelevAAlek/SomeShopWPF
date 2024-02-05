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
        private string _conStr = File.ReadAllText(@"..\..\..\Resources\MSSQLcon_str.txt");

        private Client _selectedClient;

        public ObservableCollection<Client> ClientsList { get; set; } = new ObservableCollection<Client>();
        public Client SelectedClient { get => _selectedClient; set => Set(ref _selectedClient, value); }

        public ICommand DeleteCommand { get; set; }

        public MainWindowViewModel()
        {
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecuted);
            SetClientTable();            
        }

        private bool CanDeleteCommandExecuted() => SelectedClient != null ? true : false;

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
