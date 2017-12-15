using System;
using System.ComponentModel.DataAnnotations;
using Activity.Validations;
namespace Activity.Models
{
    public class ActivityViewModel
    {
        [Display(Name = "Activity")]
        [Required]
        [MinLength(2, ErrorMessage = "Must be at least two characters long!")]
        public string ActivityName { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Must be at least ten characters long!")]
        public string Description { get; set; }

        [FutureDate(ErrorMessage = "Must be a future date")]
        public DateValues DateValues { get; set; }

        [Required]
        [ValidTime(ErrorMessage = "Not a valid time type!")]
        public string TimeType { get; set; }

        [Display(Name = "Event Duration")]
        [Required]
        public int DurationInt { get; set; }
    }

    public class DateValues
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
    }
}