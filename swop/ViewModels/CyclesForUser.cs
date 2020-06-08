using swop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace swop.ViewModels
{
    //view model to encapsulate a user with his current cycles
    public class CyclesForUser
    {
        public User user { get; set; }
        public List<Cycle> cycles { get; set; }
        public CyclesForUser(User user, List<Cycle> cycles)
        {
            this.user = user;
            this.cycles = cycles;
        }
    }
}