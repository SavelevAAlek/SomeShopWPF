using SomeShopWPF.Commands;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class EditClientViewModel : DialogViewModel
    {
        private readonly IUserDialog _userDialog;
        private string _conStr = File.ReadAllText(@"..\..\..\Resources\MSSQLcon_str.txt");
        private Client _selectedClient;

        public Client SelectedClient { get => _selectedClient; set => Set(ref _selectedClient, value); }

        public ICommand EditClientCommand { get; set; }
        public EditClientViewModel(Client selectedClient, IUserDialog userDialog)
        {
            SelectedClient = new Client(selectedClient);
            EditClientCommand = new LambdaCommand(OnEditClientCommandExecuted, CanEditClientCommandExecute);
            _userDialog = userDialog;
        }

        public EditClientViewModel() { }

        private bool CanEditClientCommandExecute()
        {
            if (_selectedClient.Surname != null &&
                _selectedClient.Name != null &&
                _selectedClient.Email != null &&
                _selectedClient.Patronymics != null) return true;

            else return false;
        }

        private void OnEditClientCommandExecuted(object? obj)
        {
            var query = "UPDATE Clients " +
                "SET Surname = @surname, \"Name\" = @name, Patronymics = @patronymics, Phone = @phone, Email = @email " +
                "WHERE Id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(_conStr))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddRange(new SqlParameter[6]
                    {
                        new SqlParameter("@id", SelectedClient.Id),
                        new SqlParameter("@surname", SelectedClient.Surname),
                        new SqlParameter("@name", SelectedClient.Name),
                        new SqlParameter("@patronymics", SelectedClient.Patronymics),
                        new SqlParameter("@phone", SelectedClient.Phone),
                        new SqlParameter("@email", SelectedClient.Email),
                    });

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                _userDialog.OpenExtraWindow(ex.Message);
            }

            _userDialog.OpenExtraWindow("Редактирование выполнено");
            OnDialogComplete(EventArgs.Empty);
        }
    }
}
