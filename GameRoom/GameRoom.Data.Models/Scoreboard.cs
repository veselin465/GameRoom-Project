using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.Data.Models
{
    public class Scoreboard
    {

        public int GameId { get; set; }
        public int PlayerId { get; set; }

        public Game Game { get; set; }
        public Player Player { get; set; }

        public int Score { get; set; }

    }
}
