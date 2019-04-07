using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.ViewModels
{
    public class ScoreboardViewModel
    {

        public int PlayerId { get; set; }
        public int GameId { get; set; }

        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public string PlayerUserName { get; set; }

        public string GameName { get; set; }

        public int Score { get; set; }

    }
}
