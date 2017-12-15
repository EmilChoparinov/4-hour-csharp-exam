using Models;
using Activity.Models;
using System.Collections.Generic;

namespace LoginReg.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Activity.Models.Activity> Activities { get; set; }
        public User()
        {
            Activities = new List<Activity.Models.Activity>();
        }
    }
}