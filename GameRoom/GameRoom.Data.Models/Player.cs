using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.Data.Models
{
    public class Player
    {

        public Player()
        {
            this.RegisteredOn = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? BeingBannedOn { get; set; }
        public string ReasonForBeingBanned { get; set; }

        public DateTime? PlayerOptedOutOn { get; set; }
        
        public string AdditionalInformation { get; set; }

        public ICollection<Scoreboard> Scoreboard { get; set; }


    }
}
