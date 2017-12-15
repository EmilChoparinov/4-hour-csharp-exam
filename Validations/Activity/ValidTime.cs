using System.ComponentModel.DataAnnotations;
using System;
using Activity.Models;
namespace Activity.Validations
{
    /// <summary>
    /// ValidTime decorator does prevalidation of information for targeted properties. Checks if the time 
    /// given is a valid string either being "Days", "Hours", or "Minutes"
    /// </summary>
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