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

    public class DbConnector
    {
        public DBInfo DBInfo { get; set; }
    }

    public static class DbConnect
    {
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