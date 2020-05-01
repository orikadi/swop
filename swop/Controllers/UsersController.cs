﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using swop.Models;

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
                    User user = db.Users.Find(id);
                    return View(user);
                }
                return HttpNotFound();
            }
            return View("Error");
            /*    if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
                */
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
            //might need to edit UserId's location in the func parameter
            //code from dominohut?
            user.Balance = 0;
            user.UserType = 0;

            foreach(User _user in db.Users)
            {
                if(_user.Email == user.Email)
                {
                    ViewBag.ErrorMassage = "email allready exist";
                    return View(user);
                }
            }
            if (ModelState.IsValid)
            {
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
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
            */
        }

        // POST: Users/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FirstName,LastName,DateOfBirth,Balance,Password,UserPicture,UserType,Country,City,Address,ApartmentPicture,ApartmentDescription,ApartmentPrice,UserId")] User user)
        {
            //might need to edit UserId's location in the func parameter
            //edited code from vgs
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
                db.Entry(db.Users.Where(x => x.UserId == user.UserId).AsQueryable().FirstOrDefault()).CurrentValues.SetValues(user);
                db.SaveChanges();
                if (IsUserAdmin() && user.UserType == 0) //if user edited themselves from admin to regular
                {
                    Login(user.Email, user.Password);
                }
                return RedirectToAction("Index", "Home"); //returns to index if edited successfully
            }
            return View(user);
            /*
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
            */
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
            /*
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
            */
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
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

        //added

        public ActionResult Login(string email, string password)
        {
            foreach (User user in db.Users)
                if (user.Email.Equals(email) && user.Password.Equals(password))
                {
                    Session["Logged"] = true;
                    Session["UserId"] = user.UserId;
                    Session["UserEmail"] = email;
                    Session["UserType"] = user.UserType;
                    //return Json(true, JsonRequestBehavior.AllowGet); 
                    return RedirectToAction("../HomePage/Index"); 
                }
            //return Json(false, JsonRequestBehavior.AllowGet);
            return RedirectToAction("../Home/Index");
        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["Logged"] = false;
            Session["UserEmail"] = null;
            Session["UserType"] = null;
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

        public ActionResult SearchUser(string email = "", string fName = "", string lName = "")
        {
            if (!IsUserLogged())
                return RedirectToAction("Index", "Home");
            List<User> uList = new List<User>();
            foreach(User user in db.Users)
            {
                if(user.Email.Contains(email) && user.FirstName.Contains(fName) && user.LastName.Contains(lName))
                {
                    uList.Add(user);
                }
            }
            return View(uList);
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



        /*      
         *      public JsonResult IsLogged()
                {
                    if (Session["Logged"] == null || (bool)Session["Logged"] == false)
                        return Json(false, JsonRequestBehavior.AllowGet);

                    var logInfo = new
                    {
                        UserEmail= Session["UserEmail"],
                        //IsAdmin = Session["IsAdmin"]
                    };
                    return Json(logInfo, JsonRequestBehavior.AllowGet);
                }
                */
    }
}
