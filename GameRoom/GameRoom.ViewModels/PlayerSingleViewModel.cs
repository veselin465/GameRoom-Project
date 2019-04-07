using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.ViewModels
{
    public class PlayerSingleViewModel
    {

        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public int Score { get; set; }
        public string AdditionalInformation { get; set; }

        public bool IsBanned { get; set; }
        public bool IsOptedOut { get; set; }

        public int LoggedInId { get; set; }

    }
}
