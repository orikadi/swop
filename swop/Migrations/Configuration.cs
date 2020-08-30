namespace swop.Migrations
{
    using swop.Models;
    using swop.Requests;
    using System;
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

            //History ADDED
            var histories = from o in context.History select o;
            foreach (var history in histories)
            {
                context.History.Remove(history);
            }
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Histories', RESEED, 0)");

            //ApartmentScore
            var apartmentScores = from o in context.ApartmentScore select o;
            foreach (var apartmentScore in apartmentScores)
            {
                context.ApartmentScore.Remove(apartmentScore);
            }
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('ApartmentScores', RESEED, 0)");

            //SaveChanges
            context.SaveChanges();
        }

        public void AddRequests(SwopContext context)
        {
            RequestHandler.Instance.AddRequest(new Request { UserId = 1, From = "Spain-Barcelona", To = "Hungary-Budapest", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 2, From = "Hungary-Budapest", To = "Canada-Toronto", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 3, From = "Canada-Toronto", To = "Israel-Petah Tikva", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 4, From = "Israel-Petah Tikva", To = "Spain-Barcelona", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 }, true);
            Request r1 = new Request { UserId = 5, From = "Israel-Petah Tikva", To = "Spain-Barcelona", Start = new DateTime(2020, 10, 1), End = new DateTime(2020, 10, 1), State = 0 };
            RequestHandler.Instance.AddRequest(r1,true);
            context.SaveChanges();

            RequestHandler.Instance.AddRequest(new Request { UserId = 6, From = "Israel-Jerusalem", To = "Italy-Sardinia", Start = new DateTime(2021, 8, 13), End = new DateTime(2021, 8, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 8, From = "Italy-Sardinia", To = "China-Beijing", Start = new DateTime(2021, 8, 13), End = new DateTime(2021, 8, 16), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 9, From = "China-Beijing", To = "Japan-Kyoto", Start = new DateTime(2021, 8, 14), End = new DateTime(2021, 8, 15), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 10, From = "Japan-Kyoto", To = "England-London", Start = new DateTime(2021, 8, 14), End = new DateTime(2021, 8, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 19, From = "Japan-Kyoto", To = "England-London", Start = new DateTime(2021, 8, 15), End = new DateTime(2021, 8, 15), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 12, From = "Egypt-Cairo", To = "Japan-Kyoto", Start = new DateTime(2021, 8, 14), End = new DateTime(2021, 8, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 11, From = "Egypt-Luxor", To = "Egypt-Cairo", Start = new DateTime(2021, 8, 15), End = new DateTime(2021, 8, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 13, From = "India- New Delhi", To = "Egypt-Luxor", Start = new DateTime(2021, 8, 14), End = new DateTime(2021, 8, 17), State = 0 }, true);

            RequestHandler.Instance.AddRequest(new Request { UserId = 23, From = "Israel-Jerusalem", To = "Italy-Sardinia", Start = new DateTime(2022, 5, 14), End = new DateTime(2022, 5, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 24, From = "Italy-Sardinia", To = "China-Beijing", Start = new DateTime(2022, 5, 14), End = new DateTime(2022, 5, 16), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 25, From = "China-Beijing", To = "Japan-Kyoto", Start = new DateTime(2022, 5, 15), End = new DateTime(2022, 5, 15), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 26, From = "Japan-Kyoto", To = "England-London", Start = new DateTime(2022, 5, 15), End = new DateTime(2022, 5, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 27, From = "Japan-Kyoto", To = "Italy-Sardinia", Start = new DateTime(2022, 5, 16), End = new DateTime(2022, 5, 16), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 28, From = "England-London", To = "Israel-Jerusalem", Start = new DateTime(2022, 5, 15), End = new DateTime(2022, 5, 19), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 29, From = "England-London", To = "Israel-Jerusalem", Start = new DateTime(2022, 5, 13), End = new DateTime(2022, 5, 19), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 31, From = "Egypt-Cairo", To = "Egypt-Luxor", Start = new DateTime(2022, 5, 15), End = new DateTime(2022, 5, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 30, From = "Egypt-Luxor", To = "India- New Delhi", Start = new DateTime(2022, 5, 16), End = new DateTime(2022, 5, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 32, From = "India- New Delhi", To = "Egypt-Cairo", Start = new DateTime(2022, 5, 15), End = new DateTime(2022, 5, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 33, From = "Israel-Tel Aviv", To = "Egypt-Cairo", Start = new DateTime(2022, 5, 14), End = new DateTime(2022, 5, 18), State = 0 }, true);

            RequestHandler.Instance.AddRequest(new Request { UserId = 34, From = "Israel-Jerusalem", To = "Italy-Sardinia", Start = new DateTime(2020, 11, 1), End = new DateTime(2020, 11, 3), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 35, From = "Egypt-Cairo", To = "China-Beijing", Start = new DateTime(2020, 11, 1), End = new DateTime(2020, 11, 2), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 36, From = "China-Beijing", To = "Japan-Kyoto", Start = new DateTime(2020, 11, 2), End = new DateTime(2020, 11, 5), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 37, From = "Japan-Kyoto", To = "England-London", Start = new DateTime(2020, 11, 2), End = new DateTime(2020, 11, 3), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 38, From = "Japan-Kyoto", To = "Italy-Sardinia", Start = new DateTime(2020, 11, 2), End = new DateTime(2020, 11, 2), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 39, From = "Israel-Tel Aviv", To = "Israel-Jerusalem", Start = new DateTime(2020, 11, 2), End = new DateTime(2020, 11, 5), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 40, From = "Israel-Tel Aviv", To = "Israel-Jerusalem", Start = new DateTime(2020, 10, 31), End = new DateTime(2020, 11, 5), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 41, From = "Italy-Sardinia", To = "Egypt-Luxor", Start = new DateTime(2020, 11, 2), End = new DateTime(2020, 11, 3), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 42, From = "Egypt-Luxor", To = "India- New Delhi", Start = new DateTime(2020, 11, 3), End = new DateTime(2020, 11, 3), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 43, From = "India- New Delhi", To = "Egypt-Cairo", Start = new DateTime(2020, 11, 2), End = new DateTime(2020, 11, 3), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 44, From = "England-London", To = "India- New Delhi", Start = new DateTime(2020, 11, 1), End = new DateTime(2020, 11, 4), State = 0 }, true);

            RequestHandler.Instance.AddRequest(new Request { UserId = 45, From = "Israel-Jerusalem", To = "Italy-Sardinia", Start = new DateTime(2021, 2, 22), End = new DateTime(2021, 2, 28), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 46, From = "Russia-Moscow", To = "China-Beijing", Start = new DateTime(2021, 2, 23), End = new DateTime(2021, 2, 28), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 47, From = "China-Beijing", To = "Japan-Kyoto", Start = new DateTime(2021, 2, 23), End = new DateTime(2021, 2, 26), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 48, From = "Japan-Kyoto", To = "England-London", Start = new DateTime(2021, 2, 23), End = new DateTime(2021, 2, 28), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 49, From = "Japan-Kyoto", To = "Italy-Sardinia", Start = new DateTime(2021, 2, 23), End = new DateTime(2021, 2, 28), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 50, From = "Israel-Tel Aviv", To = "Israel-Jerusalem", Start = new DateTime(2021, 2, 23), End = new DateTime(2021, 2, 28), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 51, From = "Israel-Tel Aviv", To = "Israel-Jerusalem", Start = new DateTime(2021, 2, 24), End = new DateTime(2021, 2, 28), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 52, From = "Usa-New York", To = "India- New Delhi", Start = new DateTime(2021, 2, 24), End = new DateTime(2021, 2, 26), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 53, From = "Egypt-Luxor", To = "Usa-New York", Start = new DateTime(2021, 2, 24), End = new DateTime(2021, 2, 28), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 54, From = "India- New Delhi", To = "England-London", Start = new DateTime(2021, 2, 23), End = new DateTime(2021, 2, 28), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 55, From = "England-London", To = "Egypt-Luxor", Start = new DateTime(2021, 2, 22), End = new DateTime(2021, 2, 28), State = 0 }, true);

            RequestHandler.Instance.AddRequest(new Request { UserId = 56, From = "Bulgaria-Sofia", To = "Italy-Sardinia", Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 13), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 57, From = "Russia-Moscow", To = "China-Beijing", Start = new DateTime(2023, 7, 3), End = new DateTime(2023, 7, 13), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 58, From = "China-Beijing", To = "Japan-Kyoto", Start = new DateTime(2023, 7, 4), End = new DateTime(2023, 7, 16), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 59, From = "Japan-Kyoto", To = "England-London", Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 15), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 60, From = "Japan-Kyoto", To = "Italy-Sardinia", Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 61, From = "Israel-Tel Aviv", To = "Israel-Jerusalem", Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 15), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 62, From = "Israel-Tel Aviv", To = "Israel-Jerusalem", Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 15), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 63, From = "Italy-Sardinia", To = "India- New Delhi", Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 13), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 64, From = "Netherlands-Amsterdam", To = "Italy-Sardinia", Start = new DateTime(2023, 7, 4), End = new DateTime(2023, 7, 14), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 65, From = "India- New Delhi", To = "England-London", Start = new DateTime(2023, 7, 4), End = new DateTime(2023, 7, 15), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 66, From = "Japan-Tokyo", To = "Netherlands-Amsterdam", Start = new DateTime(2023, 7, 3), End = new DateTime(2023, 7, 17), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 67, From = "Italy-Sardinia", To = "Japan-Kyoto", Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 14), State = 0 }, true);

            RequestHandler.Instance.AddRequest(new Request { UserId = 68, From = "Italy-Sardinia", To = "Japan-Tokyo", Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 14), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 69, From = "Japan-Tokyo", To = "India- New Delhi", Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 14), State = 0 }, true);
            RequestHandler.Instance.AddRequest(new Request { UserId = 70, From = "India- New Delhi", To = "Italy-Sardinia", Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 14), State = 0 }, true);

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
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()
            {
                Email = "yoav@gmail.com",
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
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()
            {
                Email = "neil@gmail.com",
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
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
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
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()
            {
                Email = "israel@gmail.com",
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
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 5.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//6
            {
                Email = "a@a.com",
                FirstName = "a",
                LastName = "ason",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Jerusalem",
                Address = "Jerusalem St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//7
            {
                Email = "b@b.com",
                FirstName = "b",
                LastName = "bson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "England",
                City = "London",
                Address = "London St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 7.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//8
            {
                Email = "c@c.com",
                FirstName = "c",
                LastName = "cson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Italy",
                City = "Sardinia",
                Address = "Sardinia St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//9
            {
                Email = "d@d.com",
                FirstName = "d",
                LastName = "dson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "China",
                City = "Beijing",
                Address = "Beijing St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 2.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//10
            {
                Email = "e@e.com",
                FirstName = "e",
                LastName = "eson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//11
            {
                Email = "f@f.com",
                FirstName = "f",
                LastName = "fson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Egypt",
                City = "Luxor",
                Address = "Luxor St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//12
            {
                Email = "g@g.com",
                FirstName = "g",
                LastName = "gson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Egypt",
                City = "Cairo",
                Address = "Cairo St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 10.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//13
            {
                Email = "h@h.com",
                FirstName = "h",
                LastName = "hson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "India",
                City = "New Delhi",
                Address = "New Delhi St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//14
            {
                Email = "i@i.com",
                FirstName = "i",
                LastName = "ison",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 8.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//15
            {
                Email = "j@j.com",
                FirstName = "j",
                LastName = "json",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Russia",
                City = "Moscow",
                Address = "Moscow St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 7.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//16
            {
                Email = "k@k.com",
                FirstName = "k",
                LastName = "kson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Usa",
                City = "New York",
                Address = "New York St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//17
            {
                Email = "l@l.com",
                FirstName = "l",
                LastName = "lson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Netherlands",
                City = "Amsterdam",
                Address = "Amsterdam St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//18
            {
                Email = "m@m.com",
                FirstName = "m",
                LastName = "mson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Tokyo",
                Address = "Tokyo St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//19
            {
                Email = "n@n.com",
                FirstName = "n",
                LastName = "nson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//20
            {
                Email = "o@o.com",
                FirstName = "o",
                LastName = "oson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "England",
                City = "London",
                Address = "London St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//21
            {
                Email = "q@q.com",
                FirstName = "q",
                LastName = "qson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });

            context.Users.AddOrUpdate(x => x.UserId, new User()//22
            {
                Email = "w@w.com",
                FirstName = "w",
                LastName = "wson",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Bulgaria",
                City = "Sofia",
                Address = "Sofia St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//23
            {
                Email = "1@1.com",
                FirstName = "1",
                LastName = "1son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Jerusalem",
                Address = "Jerusalem St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//24
            {
                Email = "2@2.com",
                FirstName = "2",
                LastName = "2son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Italy",
                City = "Sardinia",
                Address = "Sardinia St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//25
            {
                Email = "3@3.com",
                FirstName = "3",
                LastName = "3son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "China",
                City = "Beijing",
                Address = "Beijing St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//26
            {
                Email = "4@4.com",
                FirstName = "4",
                LastName = "4son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//27
            {
                Email = "5@5.com",
                FirstName = "5",
                LastName = "5son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//28
            {
                Email = "6@6.com",
                FirstName = "6",
                LastName = "6son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "England",
                City = "London",
                Address = "London St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//29
            {
                Email = "7@7.com",
                FirstName = "7",
                LastName = "7son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "England",
                City = "London",
                Address = "London St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//30
            {
                Email = "8@8.com",
                FirstName = "8",
                LastName = "8son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Egypt",
                City = "Luxor",
                Address = "Luxor St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//31
            {
                Email = "9@9.com",
                FirstName = "9",
                LastName = "9son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Egypt",
                City = "Cairo",
                Address = "Cairo St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//32
            {
                Email = "10@10.com",
                FirstName = "10",
                LastName = "10son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "India",
                City = "New Delhi",
                Address = "New Delhi St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//33
            {
                Email = "11@11.com",
                FirstName = "11",
                LastName = "11son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//34
            {
                Email = "12@12.com",
                FirstName = "12",
                LastName = "12son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Jerusalem",
                Address = "Jerusalem St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//35
            {
                Email = "13@13.com",
                FirstName = "13",
                LastName = "13son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Egypt",
                City = "Cairo",
                Address = "Cairo St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//36
            {
                Email = "14@14.com",
                FirstName = "14",
                LastName = "14son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "China",
                City = "Beijing",
                Address = "Beijing St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//37
            {
                Email = "15@15.com",
                FirstName = "15",
                LastName = "15son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//38
            {
                Email = "16@16.com",
                FirstName = "16",
                LastName = "16son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//39
            {
                Email = "17@17.com",
                FirstName = "17",
                LastName = "17son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//40
            {
                Email = "18@18.com",
                FirstName = "18",
                LastName = "18son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//41
            {
                Email = "19@19.com",
                FirstName = "19",
                LastName = "19son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Italy",
                City = "Sardinia",
                Address = "Sardinia St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//42
            {
                Email = "20@20.com",
                FirstName = "20",
                LastName = "20son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Egypt",
                City = "Luxor",
                Address = "Luxor St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//43
            {
                Email = "21@21.com",
                FirstName = "21",
                LastName = "21son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "India",
                City = "New Delhi",
                Address = "New Delhi St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//44
            {
                Email = "22@22.com",
                FirstName = "22",
                LastName = "22son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "England",
                City = "London",
                Address = "London St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//45
            {
                Email = "23@23.com",
                FirstName = "23",
                LastName = "23son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Jerusalem",
                Address = "Jerusalem St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//46
            {
                Email = "24@24.com",
                FirstName = "24",
                LastName = "24son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Russia",
                City = "Moscow",
                Address = "Moscow St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//47
            {
                Email = "25@25.com",
                FirstName = "25",
                LastName = "25son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "China",
                City = "Beijing",
                Address = "Beijing St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//48
            {
                Email = "26@26.com",
                FirstName = "26",
                LastName = "26son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//49
            {
                Email = "27@27.com",
                FirstName = "27",
                LastName = "27son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//50
            {
                Email = "28@28.com",
                FirstName = "28",
                LastName = "28son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//51
            {
                Email = "29@29.com",
                FirstName = "29",
                LastName = "29son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//52
            {
                Email = "30@30.com",
                FirstName = "30",
                LastName = "30son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Usa",
                City = "New York",
                Address = "New York St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//53
            {
                Email = "31@31.com",
                FirstName = "31",
                LastName = "31son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Egypt",
                City = "Luxor",
                Address = "Luxor St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//54
            {
                Email = "32@32.com",
                FirstName = "32",
                LastName = "32son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "India",
                City = "New Delhi",
                Address = "New Delhi St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//55
            {
                Email = "33@33.com",
                FirstName = "33",
                LastName = "33son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "England",
                City = "London",
                Address = "London St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//56
            {
                Email = "34@34.com",
                FirstName = "34",
                LastName = "34son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Bulgaria",
                City = "Sofia",
                Address = "Sofia St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//57
            {
                Email = "35@35.com",
                FirstName = "35",
                LastName = "35son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Russia",
                City = "Moscow",
                Address = "Moscow St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//58
            {
                Email = "36@36.com",
                FirstName = "36",
                LastName = "36son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "China",
                City = "Beijing",
                Address = "Beijing St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//59
            {
                Email = "37@37.com",
                FirstName = "37",
                LastName = "37son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//60
            {
                Email = "38@38.com",
                FirstName = "38",
                LastName = "38son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Kyoto",
                Address = "Kyoto St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//61
            {
                Email = "39@39.com",
                FirstName = "39",
                LastName = "39son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//62
            {
                Email = "40@40.com",
                FirstName = "40",
                LastName = "40son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Israel",
                City = "Tel Aviv",
                Address = "Tel Aviv St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//63
            {
                Email = "41@41.com",
                FirstName = "41",
                LastName = "41son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Italy",
                City = "Sardinia",
                Address = "Sardinia St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//64
            {
                Email = "42@42.com",
                FirstName = "42",
                LastName = "42son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Netherlands",
                City = "Amsterdam",
                Address = "Amsterdam St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//65
            {
                Email = "43@43.com",
                FirstName = "43",
                LastName = "43son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "India",
                City = "New Delhi",
                Address = "New Delhi St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//66
            {
                Email = "44@44.com",
                FirstName = "44",
                LastName = "44son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Tokyo",
                Address = "Tokyo St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//67
            {
                Email = "45@45.com",
                FirstName = "45",
                LastName = "45son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Italy",
                City = "Sardinia",
                Address = "Sardinia St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//68
            {
                Email = "46@46.com",
                FirstName = "46",
                LastName = "46son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Italy",
                City = "Sardinia",
                Address = "Sardinia St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//69
            {
                Email = "47@47.com",
                FirstName = "47",
                LastName = "47son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "Japan",
                City = "Tokyo",
                Address = "Tokyo St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.Users.AddOrUpdate(x => x.UserId, new User()//70
            {
                Email = "48@48.com",
                FirstName = "48",
                LastName = "48son",
                DateOfBirth = new DateTime(1990, 1, 1),
                Balance = 1000.0,
                Password = "123",
                UserPicture = "",
                UserType = 0,
                Country = "India",
                City = "New Delhi",
                Address = "New Delhi St",
                ApartmentPicture = "",
                ApartmentDescription = "its k i guess",
                ApartmentPrice = 3.0,
                ApartmentScore = 1.0
            });
            context.SaveChanges();

        }
    }
}
