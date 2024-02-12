using Npgsql;
using SomeShopWPF.Commands;
using SomeShopWPF.Models;
using SomeShopWPF.Services;
using SomeShopWPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SomeShopWPF.ViewModels
{
    public class PurchasesViewModel : DialogViewModel
    {
        string con = "Host=localhost;Username=postgres;Password=postgres;Database=Shop";

        private string _productToBuy;
        private readonly Client _client;
        private readonly IUserDialog _userDialog;

        private ObservableCollection<Purchase> _purchases;

        public string ProductToBuy { get => _productToBuy; set => Set(ref _productToBuy, value); } 
        public List<string> ProductNames { get; set; } = new List<string>();
        public ObservableCollection<Purchase> Purchases { get => _purchases; set => Set(ref _purchases, value); }

        public ICommand AddPurchaseCommand { get; set; }
        public PurchasesViewModel(Client selectedClient, IUserDialog userDialog)
        {
            _client = selectedClient;
            _purchases = new ObservableCollection<Purchase>(_client.Purchases);  
            _userDialog = userDialog;
            string query = "SELECT product_name FROM products_id";
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(con))
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductNames.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                _userDialog.OpenExtraWindow(ex.Message);
            }
            
            SetPurchasesTable();
            AddPurchaseCommand = new LambdaCommand(OnAddPurchaseCommandExecuted, CanAddPurchaseCommandExecute);
        }

        private bool CanAddPurchaseCommandExecute() => true;

        private void OnAddPurchaseCommandExecuted(object? obj)
        {
            string query = "INSERT INTO purchases (email, product_name)" +
                "\nVALUES (@email, @product_name)";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(con))
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddRange(new NpgsqlParameter[2]
                    {
                        new NpgsqlParameter("@email", _client.Email),
                        new NpgsqlParameter("@product_name", ProductToBuy)
                    });

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { _userDialog.OpenExtraWindow(ex.Message); }
            finally { SetPurchasesTable(); OnPropertyChanged(nameof(Purchases)); }
        }

        private void SetPurchasesTable()
        {
            Purchases = new ObservableCollection<Purchase>();
            string query = "SELECT pur.Id, pur.Email, prod.Id, prod.product_name" +
                           "\nFROM Purchases AS pur" +
                           "\nJOIN products_id as prod" +
                           "\nON prod.product_name = pur.product_name" +
                           "\nWHERE pur.Email = @email";

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
                            Purchases.Add(new Purchase(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.GetString(3)));
                    }
                }
            }
            catch (Exception ex)
            {
                _userDialog.OpenExtraWindow(ex.Message);
            }
        }


    }
}
