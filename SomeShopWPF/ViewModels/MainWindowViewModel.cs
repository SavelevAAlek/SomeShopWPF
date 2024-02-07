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

        private ObservableCollection<Client> _clientsList;

        public ObservableCollection<Client> ClientsList { get => _clientsList; set => Set(ref _clientsList, value);
    }
    public Client SelectedClient { get => _selectedClient; set => Set(ref _selectedClient, value); }

        public ICommand DeleteCommand { get; set; }
        public ICommand OpenAddWindowCommand { get; set; }

        public MainWindowViewModel(IUserDialog userDialog)
        {
            _userDialog = userDialog;
            SetClientTable();
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            OpenAddWindowCommand = new LambdaCommand(OnOpenAddWindowCommandExecuted, CanOpenAddWindowCommandExecute);
        }

        private bool CanOpenAddWindowCommandExecute() => true;

        private void OnOpenAddWindowCommandExecuted(object? obj)
        {
            _userDialog.OpenAddClientWindow();
        }


        private bool CanDeleteCommandExecute() => SelectedClient != null ? true : false;

        private void OnDeleteCommandExecuted(object? obj)
        {
            DeleteClient(SelectedClient);
            OnPropertyChanged(nameof(ClientsList));
        }

        public void SetClientTable()
        {
            string query = "SELECT * FROM Clients";

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                _clientsList = new ObservableCollection<Client>();
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

            OnPropertyChanged(nameof(ClientsList));
        }

        private void DeleteClient(Client selectedClient)
        {
            var query = $"DELETE FROM Clients WHERE Email = @email";
            

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@email", selectedClient.Email));
                command.ExecuteNonQuery();
            }

            ClientsList = new ObservableCollection<Client>();
            SetClientTable();
        }
    }
}
