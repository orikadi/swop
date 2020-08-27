using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace swop.Models
{
    public class SwopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<UserCycle> UserCycles { get; set; }
        //ADDED
        public DbSet<History> History { get; set; }
        public DbSet<ApartmentScore> ApartmentScore { get; set; }
    }
}