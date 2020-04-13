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
    }
}