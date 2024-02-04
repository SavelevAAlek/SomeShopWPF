using SomeShopWPF.Services.Implementations;
using System.Data.SqlClient;
using System.IO;

namespace SomeShopWPF.Infrastructure
{
    public class PGSQLAdapter : DataBaseWorkerService
    {
        private readonly string _conStr = File.ReadAllText(@"..\Resources\PGSQLcon_str.txt");

        PGSQLAdapter()
        {
            SqlConnection connection = new SqlConnection(_conStr);
        }
    }
}
