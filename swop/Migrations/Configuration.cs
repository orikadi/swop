﻿namespace swop.Migrations
{
    using swop.Models;
    using swop.Requests;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<swop.Models.SwopContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(swop.Models.SwopContext context)
        {
            Trunicate(context);
            AddUsers(context);
            AddRequests(context);
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

        public void AddRequests(SwopContext context)
        {
            //adding user object to new request doesnt work
            Request r1 = new Request { UserId = 1, From = "Israel-PetahTikva", To = "Spain-Barcelona", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 };
            RequestHandler.Instance.AddRequest(r1, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 2, From = "Spain-Barcelona", To = "Hungary-Budapest", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 3, From = "Hungary-Budapest", To = "Canada-Toronto", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 4, From = "Canada-Toronto", To = "Israel-PetahTikva", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 5, From = "Israel-PetahTikva", To = "Spain-Barcelona", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 },true);
            context.SaveChanges();

            Request req = context.Requests.Find(r1.UserId);
            RequestHandler.Instance.DeleteRequest(r1, false);
            context.SaveChanges();
        }
        
            public void AddUsers(SwopContext context)
        {
           
            context.Users.AddOrUpdate(x => x.UserId, new User()
            {
                Email = "ori@gmail.com",
                FirstName = "Ori",
                LastName = "Orison",
                DateOfBirth = new DateTime(1980, 12, 12),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 1,
                Country = "Spain",
                City = "Barcelona",
                Address = "Barcelona St",
                ApartmentPicture = "http://hadastal.art/wp-content/uploads/2017/06/Liors-Secret-Place11.jpg",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()
            {
                Email = "Yoav@gmail.com",
                FirstName = "Yoav",
                LastName = "Yoavson",
                DateOfBirth = new DateTime(1980, 12, 12),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 1,
                Country = "Hungary",
                City = "Budapest",
                Address = "Budapest St",
                ApartmentPicture = "http://hadastal.art/wp-content/uploads/2017/06/Liors-Secret-Place11.jpg",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()
            {
                Email = "Neil@gmail.com",
                FirstName = "Neil",
                LastName = "Neilson",
                DateOfBirth = new DateTime(1980, 12, 12),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 1,
                Country = "Canada",
                City = "Toronto",
                Address = "Toronto St",
                ApartmentPicture = "http://hadastal.art/wp-content/uploads/2017/06/Liors-Secret-Place11.jpg",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()
            {
                Email = "lior@gmail.com",
                FirstName = "Lior",
                LastName = "Liorson",
                DateOfBirth = new DateTime(1980, 12, 12),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 1,
                Country = "Israel",
                City = "Petah Tikva",
                Address = "Petah Tikva St",
                ApartmentPicture = "https://media.discordapp.net/attachments/707248831779176528/709380556290523217/Z.png",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()
            {
                Email = "IsraelIsraeli@gmail.com",
                FirstName = "Israel",
                LastName = "Israeli",
                DateOfBirth = new DateTime(1980, 12, 12),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 1,
                Country = "Israel",
                City = "Petah Tikva",
                Address = "Petah Tikva St",
                ApartmentPicture = "https://media.discordapp.net/attachments/707248831779176528/709380556290523217/Z.png",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0
            });
            context.SaveChanges();

        }
    }
}
