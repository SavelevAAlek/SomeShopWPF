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
        private string _surname;
        private string _name;
        private string _patronymics;
        private string _phone;
        private string _email;

        public string Surname { get => _surname; set => Set(ref _surname, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public string Patronymics { get => _patronymics; set => Set(ref _patronymics, value); }
        public string Phone { get => _phone; set => Set(ref _phone, value); }

        public string Email { get => _email; set => Set(ref _email, value); }

        private readonly IUserDialog _userDialog;
        private string _conStr = File.ReadAllText(@"..\..\..\Resources\MSSQLcon_str.txt");
        private Client _selectedClient;

        public ICommand EditClientCommand { get; set; }
        public EditClientViewModel(Client selectedClient, IUserDialog userDialog)
        {
            _selectedClient = selectedClient;
            _surname = selectedClient.Surname;
            _name = selectedClient.Name;
            _patronymics = selectedClient.Patronymics;
            _phone = selectedClient.Phone;
            _email = selectedClient.Email;

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
                        new SqlParameter("@id", _selectedClient.Id),
                        new SqlParameter("@surname", Surname),
                        new SqlParameter("@name", Name),
                        new SqlParameter("@patronymics", Patronymics),
                        new SqlParameter("@phone", Phone),
                        new SqlParameter("@email", Email),
                    });

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                _userDialog.OpenExtraWindow(ex.Message);
            }
            finally
            {

                OnDialogComplete(EventArgs.Empty);
            }


        }
    }
}
