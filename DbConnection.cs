using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Project.DbConnection
{
    public class DBInfo
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }

    //Root class of the appsettings.json file
    public class DbConnector
    {
        public DBInfo DBInfo { get; set; }
    }

    /// <summary>
    /// Static class to reference the appsettings.json file
    /// </summary>
    public static class DbConnect
    {
        /// <summary>
        /// Gets the connection string from appsettings.json
        /// </summary>
        /// <returns>connection string</returns>
        public static string GetDbConnectionString()
        {
            string filename = "appsettings.json";
            using (StreamReader file = File.OpenText(filename))
            {
                var json = file.ReadToEnd();
                DbConnector connector = JsonConvert.DeserializeObject<DbConnector>(json);
                return connector.DBInfo.ConnectionString;
            }
        }
    }
}