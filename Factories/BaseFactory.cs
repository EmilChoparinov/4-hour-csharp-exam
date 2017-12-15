using System.Data;
using MySql.Data.MySqlClient;
using Models;
using Project.DbConnection;
namespace Factory
{
    public interface IFactory<T> where T : BaseEntity { }

    public class SQLConnect
    {
        private string _connectionString;
        public SQLConnect()
        {
            _connectionString = DbConnect.GetDbConnectionString();
        }
        internal IDbConnection Connection
        {
            get{
                return new MySqlConnection(_connectionString);
            }
        }
    }
}