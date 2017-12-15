using System.ComponentModel.DataAnnotations;
using LoginReg.Validations;
namespace LoginReg.Models
{
    public class RegisterViewModel
    {

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(255, ErrorMessage = "255 characters max!")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(255, ErrorMessage = "255 characters max!")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email invalid!")]
        [MaxLength(255, ErrorMessage = "255 characters max!")]
        [ValidateEmailDB(ErrorMessage = "Email exists!")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(255, ErrorMessage = "255 characters max!")]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters long!")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Must have at least on letter and number!")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(255, ErrorMessage = "255 characters max!")]
        [Compare(nameof(Password), ErrorMessage = "This does not equal Password!")]
        public string ConfirmPassword { get; set; }
    }
}