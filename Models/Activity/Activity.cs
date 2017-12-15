using System;
using Models;
using LoginReg.Models;
using System.Collections.Generic;

namespace Activity.Models
{
    public class Activity : BaseEntity
    {
        public int Id { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Duration { get; set; }
        public int CoordinatorId { get; set; }
        public User Coordinator { get; set; }
        public List<Participant> Participants { get; set; }

        public Activity()
        {
            Participants = new List<Participant>();
        }
    }
}