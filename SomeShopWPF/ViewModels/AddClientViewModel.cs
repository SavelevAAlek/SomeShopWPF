using SomeShopWPF.Commands;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class AddClientViewModel : DialogViewModel
    {
        private readonly IUserDialog _userDialog;
        private string _conStr = File.ReadAllText(@"..\..\..\Resources\MSSQLcon_str.txt");
        private Client _newClient;
        public Client NewClient { get => _newClient; set => Set(ref _newClient, value); }

        public ICommand AddClientCommand { get; set; }
        public AddClientViewModel(IUserDialog userDialog)
        {
            _newClient = new Client();
            _userDialog = userDialog;
            AddClientCommand = new LambdaCommand(OnAddClientCommandExecuted, CanAddClientCommandExecute);
        }

        

        private bool CanAddClientCommandExecute()
        {
            if (_newClient.Surname != null &&
                _newClient.Name != null &&
                _newClient.Email != null &&
                _newClient.Patronymics != null) return true;

            else return false;
        }

        private void OnAddClientCommandExecuted(object? obj)
        {
            var query = "INSERT INTO Clients (Surname, \"Name\", Patronymics, Phone, Email)" +
                $"VALUES (@surname, @name, @patronymics, @phone, @email)";

            if (_newClient.Phone == null) _newClient.Phone = "Не указан";

            try
            {
                using (SqlConnection connection = new SqlConnection(_conStr))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddRange(new SqlParameter[5]
                    {
                    new SqlParameter("@surname", _newClient.Surname),
                    new SqlParameter("@name", _newClient.Name),
                    new SqlParameter("@patronymics", _newClient.Patronymics),
                    new SqlParameter("@phone", _newClient.Phone),
                    new SqlParameter("@email", _newClient.Email),
                    });

                    command.ExecuteNonQuery();
                }
            }
            catch(SqlException ex)
            {
                _userDialog.OpenExtraWindow(ex.Message);
            }

            OnDialogComplete(EventArgs.Empty);
            _newClient = new Client();
        }

    }
}
