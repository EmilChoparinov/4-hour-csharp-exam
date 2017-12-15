using System.ComponentModel.DataAnnotations;
using System;
using LoginReg.Models;
using LoginReg.Factories;
using Microsoft.AspNetCore.Identity;
namespace LoginReg.Validations
{
    /// <summary>
    /// ValidateLoginDB decorator does prevalidation of information for targeted properties. Checks if the
    /// login information given matches that of the database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ValidateLoginlDB : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            LogInfo userAttempt = (LogInfo)value;
            UserFactory userFactory = new UserFactory();
            User user = userFactory.GetUserByEmail(userAttempt.Email);
            
            //If the user does not exist in the database, the login attempt is invalid
            if (user == null) return false;
            var Hasher = new PasswordHasher<User>();

            //If the hashed passwords match the login is valid
            if (0 != Hasher.VerifyHashedPassword(user, user.Password, userAttempt.Password))
            {
                return true;
            }
            return false;
        }
    }
}