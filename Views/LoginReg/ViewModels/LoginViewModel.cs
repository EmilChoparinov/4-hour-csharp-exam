using System.ComponentModel.DataAnnotations;
using LoginReg.Validations;

namespace LoginReg.Models
{
    public class LoginViewModel
    {
        [ValidateLoginlDB(ErrorMessage = "Invalid Login!")]
        public LogInfo LogInfo { get; set; }
    }
    public class LogInfo
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}