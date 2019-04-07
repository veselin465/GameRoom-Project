using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameRoom.Data.Models
{
    public class Game
    {

        public Game()
        {
            this.RegisteredOn = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? RemovedOn { get; set; }

        public int RequiredAge { get; set; }

        public string Description { get; set; }

        public ICollection<Scoreboard> Scoreboard { get; set; }

        //public double Rating { get; set; }

    }
}
