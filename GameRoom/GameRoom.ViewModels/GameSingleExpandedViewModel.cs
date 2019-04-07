using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.ViewModels
{
    public class GameSingleExpandedViewModel
    {

        public GameSingleViewModel Game { get; set; }
        public ICollection<PlayerSingleViewModel> Players { get; set; }
        public bool IsCurrentPlayerAlreadySignedIn { get; set; }
        public int SignedInPlayerScore { get; set; }
        public bool CanSignedInPlayerCompete { get; set; }
    }
}
