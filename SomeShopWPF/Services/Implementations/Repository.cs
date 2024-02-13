using Npgsql;
using SomeShopWPF.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SomeShopWPF.Services.Implementations
{
    public class Repository : IRepository
    {
        private readonly string _mssql_con = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ConnectionString;
        private readonly string _pgsql_con = ConfigurationManager.ConnectionStrings["PGSQLConnection"].ConnectionString;
        private readonly IUserDialog _userDialog;

        public Repository(IUserDialog userDialog) => _userDialog = userDialog;

        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(Client client)
        {
            var query = "INSERT INTO Clients (Surname, \"Name\", Patronymics, Phone, Email)" +
                        "\nVALUES (@surname, @name, @patronymics, @phone, @email)";

            if (client.Phone == null) client.Phone = "Не указан";

            try
            {
                using (SqlConnection connection = new SqlConnection(_mssql_con))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddRange(new SqlParameter[5]
                    {
                    new SqlParameter("@surname", client.Surname),
                    new SqlParameter("@name", client.Name),
                    new SqlParameter("@patronymics", client.Patronymics),
                    new SqlParameter("@phone", client.Phone),
                    new SqlParameter("@email", client.Email),
                    });

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { _userDialog.OpenExtraWindow(ex.Message); }
        }

        /// <summary>
        /// Добавление покупки
        /// </summary>
        /// <param name="selectedClient"></param>
        /// <param name="product"></param>
        public void AddPurchase(Client selectedClient, string product)
        {
            string query = "INSERT INTO purchases (email, product_name)" +
                           "\nVALUES (@email, @product_name)";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_pgsql_con))
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddRange(new NpgsqlParameter[2]
                    {
                        new NpgsqlParameter("@email", selectedClient.Email),
                        new NpgsqlParameter("@product_name", product)
                    });

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { _userDialog.OpenExtraWindow(ex.Message); }
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <param name="selectedClient"></param>
        public void Delete(Client selectedClient)
        {
            var query = $"DELETE FROM Clients WHERE Email = @email";


            using (SqlConnection connection = new SqlConnection(_mssql_con))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@email", selectedClient.Email));
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Редактирование клиента
        /// </summary>
        /// <param name="selectedClient"></param>
        public void EditClient(Client selectedClient)
        {
            var query = "UPDATE Clients " +
                        "\nSET Surname = @surname, \"Name\" = @name, Patronymics = @patronymics, Phone = @phone, Email = @email " +
                        "\nWHERE Id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(_mssql_con))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddRange(new SqlParameter[6]
                    {
                        new SqlParameter("@id", selectedClient.Id),
                        new SqlParameter("@surname", selectedClient.Surname),
                        new SqlParameter("@name", selectedClient.Name),
                        new SqlParameter("@patronymics", selectedClient.Patronymics),
                        new SqlParameter("@phone", selectedClient.Phone),
                        new SqlParameter("@email", selectedClient.Email),
                    });

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                _userDialog.OpenExtraWindow(ex.Message);
            }

            _userDialog.OpenExtraWindow("Редактирование выполнено");
        }

        /// <summary>
        /// Заполнение DataGrid клиентами
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Client> GetClients()
        {
            string query = "SELECT * FROM Clients";

            using (SqlConnection connection = new SqlConnection(_mssql_con))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var client = new Client(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5));
                    yield return client;
                }
                
            }
        }

        /// <summary>
        /// Заполнение списка доступных продуктов
        /// </summary>
        /// <returns></returns>
        public List<string> GetProducts()
        {
            string query = "SELECT product_name FROM products_id";
            List<string> products = new List<string>();

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_pgsql_con))
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex) { _userDialog.OpenExtraWindow(ex.Message); }

            return products;
        }

        /// <summary>
        /// Заполнение списка покупок
        /// </summary>
        /// <param name="selectedClient"></param>
        /// <returns></returns>
        public async Task<List<Purchase>> GetPurchases(Client selectedClient)
        {
            string query = "SELECT pur.Id, pur.Email, prod.Id, prod.product_name" +
                           "\nFROM Purchases AS pur" +
                           "\nJOIN products_id as prod" +
                           "\nON prod.product_name = pur.product_name" +
                           "\nWHERE pur.Email = @email";

            List<Purchase> purchases = new List<Purchase>();

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_pgsql_con))
                {
                    await connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.Add(new NpgsqlParameter("@email", selectedClient.Email));
                    
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        purchases.Add(new Purchase(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetString(3)));
                    }
                }
            }
            catch (Exception ex) { _userDialog.OpenExtraWindow(ex.Message); }

            return purchases;
        }
    }
}
