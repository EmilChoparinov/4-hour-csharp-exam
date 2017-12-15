using Models;
using LoginReg.Models;
namespace Activity.Models
{
    public class Participant : BaseEntity
    {
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Activity Activity { get; set; }
    }
}