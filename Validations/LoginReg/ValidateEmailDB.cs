using System.ComponentModel.DataAnnotations;
using System;
using LoginReg.Models;
using LoginReg.Factories;
namespace LoginReg.Validations
{
    /// <summary>
    /// ValidateEmailDB decorator does prevalidation of information for targeted properties. Checks if the
    /// email given is currently in the database;
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ValidateEmailDB : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            UserFactory userFactory = new UserFactory();
            
            //really only checks for if email exists
            User user = userFactory.GetUserByEmail((string)value);
            if (user == null) return true;
            return false;
        }
    }
}