using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.Services
{
    public class CurrentSignedPlayer
    {
        /// <summary>
        /// keeps globally information about last successful logged in user's id
        /// </summary>
        public static int PlayerId { get; set; } = 0;

    }
}
