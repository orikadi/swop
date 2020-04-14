using System;
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
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
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
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,FirstName,LastName,DateOfBirth,Balance,Password,UserPicture,UserType,Country,City,Address,ApartmentPicture,ApartmentDescription,ApartmentPrice")] User user)
        {
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
        public ActionResult Edit(string id)
        {
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
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FirstName,LastName,DateOfBirth,Balance,Password,UserPicture,UserType,Country,City,Address,ApartmentPicture,ApartmentDescription,ApartmentPrice")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
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
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
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
                    //Session["UserId"] = user.Id;
                    Session["User"] = email;
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
            Session["User"] = null;
            Session["UserType"] = null;
            return RedirectToAction("Index", "Home");
        }

        public JsonResult IsLogged()
        {
            if (Session["Logged"] == null || (bool)Session["Logged"] == false)
                return Json(false, JsonRequestBehavior.AllowGet);

            var logInfo = new
            {
                UserName = Session["User"],
                //IsAdmin = Session["IsAdmin"]
            };
            return Json(logInfo, JsonRequestBehavior.AllowGet);
        }
    }
}
