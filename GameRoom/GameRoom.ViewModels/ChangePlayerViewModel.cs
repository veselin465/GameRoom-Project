using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.ViewModels
{
    public class ChangePlayerViewModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string AdditionalInformation { get; set; }

    }
}
