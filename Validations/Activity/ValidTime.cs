using System.ComponentModel.DataAnnotations;
using System;
using Activity.Models;
namespace Activity.Validations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ValidTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string givenTime = (string)value;
            if (givenTime == "Days" || givenTime == "Hours" || givenTime == "Minutes")
            {
                return true;
            }
            return false;
        }
    }
}