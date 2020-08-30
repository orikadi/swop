using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using swop.Models;
using swop.Requests;
using swop.ViewModels;

namespace swop.Controllers
{
    public class UsersController : Controller
    {
        private SwopContext db = new SwopContext();
        
        
        // GET: Users
        public ActionResult Index()
        {
            if (IsUserLogged()) //only logged users can view user list
                return View(db.Users.ToList());
            else
                return View("Error");
            // return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (IsUserLogged())
            {
                if (UserExists(id.GetValueOrDefault(0)))
                {
                    User user = db.Users.Where(u => u.UserId == id).Include(x => x.ApartmentScores).First();
                    return View(user);
                }
                return HttpNotFound();
            }
            return View("Error");
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,FirstName,LastName,DateOfBirth,Balance,Password,UserPicture,UserType,Country,City,Address,ApartmentPicture,ApartmentDescription,ApartmentPrice,UserId")] User user)
        {
            user.Balance = 0;
            user.UserType = 0;
            ModelState.Remove("ApartmentScore");
            user.ApartmentScore = 1;
           
            foreach(User _user in db.Users)
            {
                if(_user.Email == user.Email)
                {
                    ViewBag.ErrorMessage = "email already exists";
                    return View(user);
                }
            }
            
            if (ModelState.IsValid)
            {
                //upload pictures to db and change user's pic paths to server pic paths
                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("../HomePage/Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id) 
        {
            if (UserExists(id.GetValueOrDefault(0)))
            {
                if (IsAllowedToEdit(id.GetValueOrDefault(0)))
                {
                    User user = db.Users.Find(id);
                    return View(user);
                }
                return View("Error");
            }
            return HttpNotFound();
        }

        // POST: Users/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FirstName,LastName,DateOfBirth,Password,UserType,Country,City,Address,ApartmentDescription,ApartmentPrice,UserId")] User user)
        {
            bool flag = false;
            foreach (User item in db.Users)
            {
                if (item.UserId != user.UserId && item.Email.Equals(user.Email))
                {
                    flag = true;
                    ViewBag.EmailError = "Email is taken";
                }
            }
            if (!flag)
            {   
                User sameUser = db.Users.Find(user.UserId); //is used because otherwise elements that cant be changed will be null in db
                user.Country = sameUser.Country;
                user.City = sameUser.City;
                user.Address = sameUser.Address;
                user.Balance = sameUser.Balance;
                user.UserPicture = sameUser.UserPicture;
                user.ApartmentPicture = sameUser.ApartmentPicture;
                
                db.Entry(db.Users.Where(x => x.UserId == user.UserId).AsQueryable().FirstOrDefault()).CurrentValues.SetValues(user);
                db.SaveChanges();
                if (IsUserAdmin() && user.UserType == 0) //if user edited themselves from admin to regular
                {
                    Login(user.Email, user.Password);
                }
                return RedirectToAction("../HomePage"); //returns to index if edited successfully
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (UserExists(id.GetValueOrDefault(0)))
            {
                if (IsAllowedToEdit(id.GetValueOrDefault(0)))
                {
                    User user = db.Users.Find(id);
                    return View(user);
                }
                return View("Error");
            }
            return HttpNotFound();
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            //delete pictures
            string filePathUser = Server.MapPath("~/Uploads/UserPictures/" + user.UserPicture);
            string filePathApartment = Server.MapPath("~/Uploads/ApartmentPictures/" + user.ApartmentPicture);
            if (System.IO.File.Exists(filePathUser))
            {
                System.IO.File.Delete(filePathUser);
            }
            if (System.IO.File.Exists(filePathApartment))
            {
                System.IO.File.Delete(filePathApartment);
            }
            //delete user
            db.Users.Remove(user);
            db.SaveChanges();
      
            if (id == (int)Session["UserId"])
                return Logout();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login(string email, string password)
        {
            foreach (User user in db.Users)
                if (user.Email.Equals(email) && user.Password.Equals(password))
                {
                    Session["Logged"] = true;
                    Session["UserId"] = user.UserId;
                    Session["UserEmail"] = email;
                    Session["UserType"] = user.UserType;
                    Session["HasActiveRequest"] = false;
                    if (db.Requests.Where(r => (r.UserId == user.UserId && r.State == 0)).Any())
                    {
                        Session["HasActiveRequest"] = true;
                        //check if user is locked into a cycle and if so get the cycle id
                        if (db.UserCycles.Where(uc => uc.UserId == user.UserId && uc.IsLocked).Any())
                            Session["LockedInCycleID"] = db.UserCycles.Where(uc => uc.UserId == user.UserId && uc.IsLocked).First().CycleId.ToString();
                    }
                    return RedirectToAction("../HomePage/Index"); 
                }
            return RedirectToAction("../Home/Index");
        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["Logged"] = false;
            Session["UserEmail"] = null;
            Session["UserType"] = null;
            Session["HasActiveRequest"] = null;
            Session["LockedInCycleID"] = null;
            return RedirectToAction("Index", "Home");
        }
        //adding funds to user
        public ActionResult AddFundsPage()
        {
            if ((Session["Logged"].Equals(true)))
            {
                int userId = (int)Session["UserId"];
                User user = db.Users.Find(userId);
                return View(user);
            }
            return RedirectToAction("Error");
        }

        public JsonResult AddFunds(double? toAdd)
        {
            int? userId = (int)Session["UserId"];
            User user = db.Users.Find(userId);
            if (toAdd.HasValue)
            {
                if (toAdd.Value >= 0 && toAdd.Value <= 10000)
                {

                    user.Balance += Math.Round(toAdd.Value, 2, MidpointRounding.AwayFromZero);
                    db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult EditUserPicturePage()
        {
            if ((Session["Logged"].Equals(true)))
            {
                int userId = (int)Session["UserId"];
                User user = db.Users.Find(userId);
                return View(user);
            }
            return RedirectToAction("Error");
        }

        public JsonResult EditUserPicture()
        {
            // check if the user selected a file to upload
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    //user id is passed as the file's name
                    int userId = Int32.Parse(files.AllKeys[0]);
                    User user = db.Users.Find(userId);
                    if (user == null)
                        return Json("bad user id");
                    string fileName = "User" + userId + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads/UserPictures/"), fileName);
                    // save the file
                    file.SaveAs(path);
                    //change UserPicture path in db
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("File uploaded successfully");
                }
                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }

        public JsonResult DeleteUserPicture(int userId)
        {
            if (IsAllowedToEdit(userId))
            {
                User user = db.Users.Find(userId);
                //delete picture
                string filePath = Server.MapPath("~/Uploads/UserPictures/" + user.UserPicture);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                user.UserPicture = ""; //overwrite image path in db
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditApartmentPicturePage()
        {
            if ((Session["Logged"].Equals(true)))
            {
                int userId = (int)Session["UserId"];
                User user = db.Users.Find(userId);
                return View(user);
            }
            return RedirectToAction("Error");
        }

        public JsonResult EditApartmentPicture()
        {
            // check if the user selected a file to upload
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    //user id is passed as the file's name
                    int userId = Int32.Parse(files.AllKeys[0]);
                    User user = db.Users.Find(userId);
                    if (user == null)
                        return Json("bad user id");
                    string fileName = "Apartment" + userId + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads/ApartmentPictures/"), fileName);
                    // save the file
                    file.SaveAs(path);
                    //change UserPicture path in db
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("File uploaded successfully");
                }
                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }

        public JsonResult DeleteApartmentPicture(int userId)
        {
            if (IsAllowedToEdit(userId))
            {
                User user = db.Users.Find(userId);
                //delete picture
                string filePath = Server.MapPath("~/Uploads/ApartmentPictures/" + user.ApartmentPicture);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                user.ApartmentPicture = ""; //overwrite image path in db
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchUser(string email = "", string country = "", string city = "", double price = int.MaxValue)
        {
            if (!IsUserLogged())
                return RedirectToAction("Index", "Home");
            List<User> uList = new List<User>();
            foreach(User user in db.Users)
            {
                if ((user.Email.IndexOf(email, StringComparison.OrdinalIgnoreCase) >=0) && (user.Country.IndexOf(country, StringComparison.OrdinalIgnoreCase) >= 0) && (user.City.IndexOf(city, StringComparison.OrdinalIgnoreCase) >= 0) && (user.ApartmentPrice <= price))
                {
                    uList.Add(user);
                }
            }
            return View(uList);
        }

        //cycle search page
        public ActionResult RequestCycleSearch(int? id)
        {
             if ((Session["Logged"].Equals(true)))
             {
                 int userId = (int)Session["UserId"];
                 User user = db.Users.Find(userId);
                 return View(user);
             }
             return RedirectToAction("Error");
        }

        public JsonResult SearchRequest(string dest, string start, string end)
        {
            int? userId = (int)Session["UserId"];
            User user = db.Users.Find(userId);
            //check if has an active request already
            if (!db.Requests.Where(r => (r.UserId == userId && r.State == 0)).Any())
            {
                int[] sDate = Array.ConvertAll(start.Split('-'), s => int.Parse(s));
                int[] eDate = Array.ConvertAll(end.Split('-'), s => int.Parse(s));
                Request r = new Request{ UserId = user.UserId, From = user.Country + "-" + user.City, To = dest, Start = new DateTime(sDate[0], sDate[1], sDate[2]), End = new DateTime(eDate[0], eDate[1], eDate[2]), State = 0 };
                RequestHandler.Instance.AddRequest(r, true);
                Session["HasActiveRequest"] = true;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyCycles(int? id)
        {
            if ((Session["Logged"].Equals(true)))
            {
                int userId = (int)Session["UserId"];
                User user = db.Users.Where(u => u.UserId == userId).Include(u => u.Requests).First();
                //update session whether user still has an active request (request wasnt completed while logged in)
                if ((bool)Session["HasActiveRequest"]) 
                {
                    if (!db.Requests.Where(r => (r.UserId == user.UserId && r.State == 0)).Any())
                        Session["HasActiveRequest"] = false;
                }
                //go through usercycles, collect the cycle ids of the usercycles with same userid as this user
                List<int> cycleIds;
                cycleIds = db.UserCycles.Where(uc => uc.UserId == userId).Select(uc => uc.CycleId).ToList();

                List<Cycle> cycles = db.Cycles.Where(c => cycleIds.Contains(c.CycleId)).ToList();
                List<User> hosts = new List<User>();
                

                if (Session["LockedInCycleID"] != null && Session["LockedInCycleID"].ToString() != "") 
                {
                    Cycle lockedCycle = db.Cycles.Find(Int32.Parse(Session["LockedInCycleID"].ToString()));
                    cycles.Remove(lockedCycle);
                    CyclesForUser userCycles = new CyclesForUser(user, cycles, lockedCycle);
                    return View(userCycles);
                }
                else
                {
                    CyclesForUser userCycles = new CyclesForUser(user, cycles, null);
                    return View(userCycles);
                }
            }
            return RedirectToAction("Error");
        }
        
        public ActionResult DeleteRequest(int? id)
        {
            int userId = (int)Session["UserId"];
            User user = db.Users.Find(userId);
            if (user == null)
                HttpNotFound();
           
            CyclesController.mutex.WaitOne();
            //check if request is still valid
            if (!db.Requests.Where(r => (r.UserId == user.UserId && r.State == 0)).Any())
            {
                CyclesController.mutex.ReleaseMutex();
                View("Error");
            }

            //get an ongoing request
            Session["HasActiveRequest"] = false;
            Request req = db.Requests.Where(r => (r.UserId == userId && r.State == 0)).First();
            //delete request
            RequestHandler.Instance.DeleteRequest(req, false);
            CyclesController.mutex.ReleaseMutex();
            return RedirectToAction("../HomePage/Index");
        }

        public ActionResult MyHistory(int? id)//ADDED
        {
            if ((Session["Logged"].Equals(true)))
            {
                int userId = (int)Session["UserId"];
                User user = db.Users.Where(u => u.UserId == userId)
                    .Include(x=>x.Histories.Select(o => o.Host).Select(y => y.ApartmentScores))
                    .Include(x=>x.Histories.Select(o=>o.Guest))
                    .First();
                return View(user);
            }
            return RedirectToAction("Error");
        }

        public ActionResult AddScorePage(int? id)
        {
            if (!(Session["Logged"].Equals(true)))
            {
                return RedirectToAction("Error");
            }
            int userId = (int)Session["UserId"];
            User userToScore = db.Users.Where(u => u.UserId == id).Include(u => u.ApartmentScores.Select(apS => apS.ScoreByUser)).First();
            foreach(ApartmentScore apS in userToScore.ApartmentScores)
            {
                if(apS.ScoreByUser.UserId == userId)
                    return RedirectToAction("MyHistory");
            }
            return View(userToScore);
        }

        public JsonResult GiveScore(double? toAdd, int? scoredUserId)
        {
            int? userId = (int)Session["UserId"];
            User user = db.Users.Find(userId);
            User scoredUser = db.Users.Where(u => u.UserId == scoredUserId).Include(x => x.ApartmentScores).First();
            if (toAdd.HasValue)
            {
                if (toAdd.Value >= 1 && toAdd.Value <= 5)
                {
                    ApartmentScore ap = new ApartmentScore
                    {
                        ScoreByUser = user,
                        Score = toAdd.Value,
                        UserId = scoredUser.UserId,
                        User = scoredUser
                    };
                    scoredUser.ApartmentScores.Add(ap);
                    double sum = 0;
                    foreach(ApartmentScore apartmentS in scoredUser.ApartmentScores)
                    {
                        sum += apartmentS.Score;
                    }
                    scoredUser.ApartmentScore = sum / scoredUser.ApartmentScores.Count;
                    db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        //Permissions check functions
        private bool IsUserLogged()
        {
            if (Session["Logged"] == null)
                return false;
            return (bool)Session["Logged"];
        }

        private bool IsUserAdmin()
        {
            if (IsUserLogged())
                return (int)Session["UserType"] == 1;
            return false;
        }

        private bool IsAllowedToEdit(int id)
        {
            if (IsUserLogged())
            {
                if (IsUserAdmin())
                    return true;
                else
                    return (int)Session["UserId"] == id;
            }
            return false;
        }
        private bool UserExists(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
                return false;
            return true;
        }

        private User GetHost(User user, Cycle cycle)
        {
            Request userReq = db.Requests.Where(r => (r.UserId == user.UserId && r.State == 0)).First();
            string userDest = userReq.To; //got user destination through an active request

            User host = new User();
            foreach (UserCycle uc in cycle.UserCycles)
            {
                //User ucUser = db.Users.Find(uc.UserId); //uc.User is null so ucUser is utilized
                User ucUser = db.Users.Where(u => u.UserId == uc.UserId).Include(x => x.ApartmentScores).First();
                Request req = db.Requests.Where(r => (r.UserId == ucUser.UserId && r.State == 0)).First();
                string dest = req.To;
                string residence = ucUser.Country + "-" + ucUser.City;
                if (residence == userDest)
                    host = ucUser;
            }
            return host;
        }

        public ActionResult Error(int? id)
        {
            return View();
        }
    }
}
