using SomeShopWPF.Services.Implementations;
using System.Data.SqlClient;
using System.IO;

namespace SomeShopWPF.Infrastructure
{
    public class MSSQLAdapter : DataBaseWorkerService
    {
        private readonly string _conStr = File.ReadAllText(@"..\Resources\MSSQLcon_str.txt");

        MSSQLAdapter()
        {
            SqlConnection connection = new SqlConnection(_conStr);
        }
    }
}
