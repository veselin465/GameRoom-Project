using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.ViewModels
{
    public class PlayerSingleExpandedViewModel
    {

        public PlayerSingleViewModel Player{get;set ;}
        public ICollection<GameSingleViewModel> Games { get; set; }

        public int Age { get; set; }
        public bool IsOptedOut { get; set; }
        public bool IsBanned { get; set; }
        public string ReasonForBeingBanned { get; set; }
    }
}
