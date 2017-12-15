using System.ComponentModel.DataAnnotations;
using System;
using Activity.Models;
namespace Activity.Validations
{
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