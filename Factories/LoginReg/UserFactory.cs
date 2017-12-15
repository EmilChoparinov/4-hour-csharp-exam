using System.Data;
using Factory;
using LoginReg.Models;
using Dapper;
using System.Linq;
using System;
using System.Collections.Generic;
using Activity.Models;
namespace LoginReg.Factories
{
    public class UserFactory : SQLConnect, IFactory<User>
    {
        public void AddUser(User user)
        {
            using (IDbConnection dbConnnection = Connection)
            {
                string query = "INSERT INTO users (FirstName, LastName, Email, Password, CreatedAt) VALUES (@FirstName, @LastName, @Email, @Password, NOW())";
                dbConnnection.Open();
                dbConnnection.Execute(query, user);
            }
        }

        public User GetUserByEmail(string Email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT * FROM users WHERE users.Email = '{Email}'";
                dbConnection.Open();
                return dbConnection.Query<User>(query).FirstOrDefault();
            }
        }

        public User GetUserByID(int Id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT * FROM users WHERE users.Id = {Id}";
                dbConnection.Open();
                return dbConnection.Query<User>(query).FirstOrDefault();
            }
        }

        //         public IEnumerable<Activity.Models.Activity> GetUserActivities(int Id){
        //     using(IDbConnection db = Connection){
        //         string query = $"SELECT * FROM users JOIN participants ON users.Id = participants.UserId JOIN activities ON activities.Id = participants.ActivityId;";

        //         var activities 
        //     }
        // }
    }
}