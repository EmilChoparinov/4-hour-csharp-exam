using System.ComponentModel.DataAnnotations;
using System;
using LoginReg.Models;
using LoginReg.Factories;
using Microsoft.AspNetCore.Identity;
namespace LoginReg.Validations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ValidateLoginlDB : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            LogInfo userAttempt = (LogInfo) value;
            UserFactory userFactory = new UserFactory();
            User user = userFactory.GetUserByEmail(userAttempt.Email);
            if(user == null) return false;
            var Hasher = new PasswordHasher<User>();
            if(0 != Hasher.VerifyHashedPassword(user, user.Password, userAttempt.Password)){
                return true;
            }
            return false;
        }
    }
}