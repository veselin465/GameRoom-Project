using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.ViewModels
{
    public class GameSingleViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; }
        
        public int RequiredAge { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }

        public int LoggedInId { get; set; }
        public bool IsRemoved { get; set; }

    }
}
