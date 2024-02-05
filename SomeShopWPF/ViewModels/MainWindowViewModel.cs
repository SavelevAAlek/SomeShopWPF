using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Documents;

namespace SomeShopWPF.ViewModels
{
    public class MainWindowViewModel : DialogViewModel
    {
        private string _conStr = File.ReadAllText(@"..\..\..\Resources\MSSQLcon_str.txt");

        public ObservableCollection<Client> ClientsList { get; set; } = new ObservableCollection<Client>();
        public MainWindowViewModel()
        {
            SetClientTable();            
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
    }
}
