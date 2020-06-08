using swop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace swop.ViewModels
{
    //viewmodel to encapuslate all info needed for the cycle info page
    public class CycleInfoForUser
    {
        public User user, guest, host;
        public Cycle cycle;

        public CycleInfoForUser(User user, User guest, User host, Cycle cycle)
        {
            this.user = user;
            this.guest = guest;
            this.host = host;
            this.cycle = cycle;
        }
    }
}