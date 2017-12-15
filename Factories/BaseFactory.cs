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

        /// <summary>
        /// Created a connection to MySQL server
        /// </summary>
        public SQLConnect()
        {
            //GetDbConnectionString is a method that grabs appsettings.json DBInfo line
            _connectionString = DbConnect.GetDbConnectionString();
        }

        /// <summary>
        /// Generate a MySQL connection based on _connectionString;
        /// </summary>
        /// <returns>IDBConnection</returns>
        internal IDbConnection Connection
        {
            get{
                return new MySqlConnection(_connectionString);
            }
        }
    }
}