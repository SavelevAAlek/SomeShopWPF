using Npgsql;
using SomeShopWPF.Models;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SomeShopWPF.ViewModels
{
    public class PurchasesViewModel : DialogViewModel
    {
        private readonly Client _client;
        public IEnumerable<Purchase> Purchases { get => _client.Purchases; }
        public PurchasesViewModel(Client selectedClient)
        {
            _client = selectedClient;
            SetPurchasesTable();
        }


        private void SetPurchasesTable()
        {
            string query = "SELECT * FROM Purchases WHERE Email = @email";
            string con = "Host=localhost;Port=5433;Username=postgres;Password=QuiteMissHome13.;Database=Shop";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(con))
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.Add(new NpgsqlParameter("@email", _client.Email));

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        _client.Purchases.Add(new Purchase(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.GetString(3)));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
