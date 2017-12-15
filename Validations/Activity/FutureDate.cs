using System.ComponentModel.DataAnnotations;
using System;
using Activity.Models;
namespace Activity.Validations
{
    /// <summary>
    /// FutureDate decorator does prevalidation of information for targeted properties. Checks if the date is 
    /// less that the current date
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateValues dateValues = (DateValues) value;
            DateTime date = dateValues.Date.Add(dateValues.Time.TimeOfDay);
            if(date > DateTime.Now) return true;
            return false;
        }
    }
}