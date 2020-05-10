namespace swop.Migrations
{
    using swop.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<swop.Models.SwopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(swop.Models.SwopContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }

        private void Trunicate(SwopContext context)
        {
            //Users
            var users = from o in context.Users select o;
            foreach (var user in users)
            {
                context.Users.Remove(user);
            }
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Users', RESEED, 0)");

            //Request
            var requests = from o in context.Requests select o;
            foreach (var request in requests)
            {
                context.Requests.Remove(request);
            }
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Requests', RESEED, 0)");

            //Cycle
            var cycles = from o in context.Cycles select o;
            foreach (var cycle in cycles)
            {
                context.Cycles.Remove(cycle);
            }
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Cycles', RESEED, 0)");

            //UserCycles
            var userCycles = from o in context.UserCycles select o;
            foreach (var userCycle in userCycles)
            {
                context.UserCycles.Remove(userCycle);
            }
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('UserCycles', RESEED, 0)");

            //SaveChanges
            context.SaveChanges();
        }
    }
}
