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
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">User object to be given</param>
        public void AddUser(User user)
        {
            using (IDbConnection dbConnnection = Connection)
            {
                string query = "INSERT INTO users (FirstName, LastName, Email, Password, CreatedAt) VALUES (@FirstName, @LastName, @Email, @Password, NOW())";
                dbConnnection.Open();
                dbConnnection.Execute(query, user);
            }
        }

        /// <summary>
        /// Gets a unique user by their email
        /// </summary>
        /// <param name="Email">Email of the requested user</param>
        /// <returns>User</returns>
        public User GetUserByEmail(string Email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT * FROM users WHERE users.Email = '{Email}'";
                dbConnection.Open();
                return dbConnection.Query<User>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a unique user by ther Id
        /// </summary>
        /// <param name="Id">Id of the user</param>
        /// <returns></returns>
        public User GetUserByID(int Id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT * FROM users WHERE users.Id = {Id}";
                dbConnection.Open();
                return dbConnection.Query<User>(query).FirstOrDefault();
            }
        }
    }
}