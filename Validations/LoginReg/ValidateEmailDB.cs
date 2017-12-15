using System.ComponentModel.DataAnnotations;
using System;
using LoginReg.Models;
using LoginReg.Factories;
namespace LoginReg.Validations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ValidateEmailDB : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            UserFactory userFactory = new UserFactory();
            User user = userFactory.GetUserByEmail((string) value);
            if(user == null) return true;
            return false;
        }
    }
}