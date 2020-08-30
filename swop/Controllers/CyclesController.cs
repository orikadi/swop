using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using swop.Models;
using swop.Requests;
using swop.ViewModels;

namespace swop.Controllers
{
    public class CyclesController : Controller
    {
        private SwopContext db = new SwopContext();
        public static Mutex mutex = new Mutex();

        // GET: Cycles
        public ActionResult Index()
        {
            return View(db.Cycles.ToList());
        }

        // GET: Cycles/Details/5
        public ActionResult Details(int? cid, int? uid)
        {
            if (cid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cycle cycle = db.Cycles.Find(cid);
            if (cycle == null)
            {
                return HttpNotFound();
            }
            User user = db.Users.Find(uid);

            //find user's host and guest by comparing residences with destinations
            Request userReq = db.Requests.Where(r => (r.UserId == user.UserId && r.State == 0)).First();
            string userDest = userReq.To; //got user destination through an active request
            string userResidence = user.Country + "-" + user.City;
          
            User host = new User(), guest = new User();
            foreach (UserCycle uc in cycle.UserCycles)
            {
                //User ucUser = db.Users.Find(uc.UserId); //uc.User is null so ucUser is utilized
                User ucUser = db.Users.Where(u => u.UserId == uc.UserId).Include(x => x.ApartmentScores).First();
                Request req = db.Requests.Where(r => (r.UserId == ucUser.UserId && r.State == 0)).First();
                string dest = req.To;
                string residence = ucUser.Country + "-" + ucUser.City;
                if (residence == userDest)
                    host = ucUser;
                if (userResidence == dest)
                    guest = ucUser;
            }
            
            CycleInfoForUser cycleInfo = new CycleInfoForUser(user, guest, host, cycle);
            return View(cycleInfo);
        }

        // GET: Cycles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cycles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CycleId,Start,End")] Cycle cycle)
        {
            if (ModelState.IsValid)
            {
                db.Cycles.Add(cycle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cycle);
        }

        // GET: Cycles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cycle cycle = db.Cycles.Find(id);
            if (cycle == null)
            {
                return HttpNotFound();
            }
            return View(cycle);
        }

        // POST: Cycles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CycleId,Start,End")] Cycle cycle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cycle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cycle);
        }

        // GET: Cycles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cycle cycle = db.Cycles.Find(id);
            if (cycle == null)
            {
                return HttpNotFound();
            }
            return View(cycle);
        }

        // POST: Cycles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cycle cycle = db.Cycles.Find(id);
            db.Cycles.Remove(cycle);
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

        //calculate current percentage of cycle locking
        public JsonResult PercentageComplete(int? id)
        {
            if (id == null)
            {
                
            }
            //get all user cycles of this cycle
            List<UserCycle> ucs = db.UserCycles.Where(uc => uc.CycleId == id).ToList();
            double lockedCount = 0.0;
            foreach(UserCycle uc in ucs)
            {
                if (uc.IsLocked)
                    lockedCount++;
            }
            double percent = (lockedCount / ucs.Count) * 100;
            return Json(percent, JsonRequestBehavior.AllowGet);
        }


        //locking into a cycle (after funds were validated)
        public JsonResult LockIn(int? cid, int? uid)
        {
            Cycle cycle = db.Cycles.Find(cid);
            User user = db.Users.Find(uid);
            if (user == null || cycle == null)
            {
                
            }
            //check if user is already locked into a cycle
            if (db.UserCycles.Where(uc => uc.UserId == user.UserId && uc.IsLocked).Any())
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            //get user cycle and change lock state to true
            UserCycle userCycle = db.UserCycles.Where(uc => uc.CycleId == cycle.CycleId && uc.UserId == user.UserId).First();
            userCycle.IsLocked = true;
            db.Entry(userCycle).Property("IsLocked").IsModified = true;
            db.SaveChanges();
            //check if cycle is now complete and return the right indicator
            if (IsCycleComplete(cycle))
            {
                
                foreach(UserCycle uc in cycle.UserCycles)
                {
                    int userId = uc.UserId;
                    User userForSearch = uc.User;
                    //finding guest and dest 
                    Request userReq = db.Requests.Where(r => (r.UserId == userId && r.State == 0)).First();
                    string userDest = userReq.To; //got user destination through an active request
                    string userResidence = userForSearch.Country + "-" + userForSearch.City;

                    User host = null;
                    User guest = null;
                    foreach (UserCycle uc2 in cycle.UserCycles)
                    {
                        User ucUser = db.Users.Find(uc2.UserId); //uc.User is null so ucUser is utilized
                        Request req = db.Requests.Where(r => (r.UserId == ucUser.UserId && r.State == 0)).First();
                        string dest = req.To;
                        string residence = ucUser.Country + "-" + ucUser.City;
                        if (residence == userDest)
                            host = ucUser;
                        if (userResidence == dest)
                            guest = ucUser;
                    }

                    User use = db.Users.Where(u => u.UserId == userId).Include(a=>a.Histories).First();
                    History h = new History
                    {
                        UserId = userId,
                        User = use,
                        StartDate = cycle.Start.Date,
                        EndDate = cycle.End.Date,
                        Host = host,
                        Guest = guest
                    };
                    db.History.Add(h);
                    if (use.Histories == null)
                    {
                        use.Histories = new List<History>();
                    }
                    use.Histories.Add(h);
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(2, JsonRequestBehavior.AllowGet);
        }

        //locking out of a cycle
        public JsonResult LockOut(int? cid, int? uid)
        {
            Cycle cycle = db.Cycles.Find(cid);
            User user = db.Users.Find(uid);
            if (user == null || cycle == null)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }

            mutex.WaitOne();
            //check if request is still valid
            if (!db.Requests.Where(r => (r.UserId == user.UserId && r.State == 0)).Any()) 
            {
                mutex.ReleaseMutex();
                return Json(-1, JsonRequestBehavior.AllowGet);
            }


            //get user cycle and change lock state to false
            UserCycle userCycle = db.UserCycles.Where(uc => uc.CycleId == cycle.CycleId && uc.UserId == user.UserId).First();
            userCycle.IsLocked = false;
            db.Entry(userCycle).Property("IsLocked").IsModified = true;
            db.SaveChanges();
            mutex.ReleaseMutex();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        //passes funds to all cycle members, marks all requests as complete
        public JsonResult CompleteCycle(int? cid)
        {
            Cycle cycle = db.Cycles.Find(cid);

            mutex.WaitOne();

            //check if all usercycles involved are still locked in   
            List<UserCycle> ucs = db.UserCycles.Where(uc => uc.CycleId == cycle.CycleId).Include(uc => uc.User).ToList();
            foreach(UserCycle uc in ucs)
            {
               if (!uc.IsLocked)
                {
                    mutex.ReleaseMutex();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }   
            }
            //transfer funds from and to each user, and mark requests as complete
            foreach(UserCycle uc in ucs)
            {
                User user = db.Users.Find(uc.UserId);
                Request request = db.Requests.Where(r => (r.UserId == user.UserId && r.State == 0 && r.Start == cycle.Start && r.End == cycle.End)).First(); //user's current request
                //find host (by comparing residence with destination) and transfer funds to him
                string destCountry = request.To.Split('-')[0];
                string destCity = request.To.Split('-')[1];

                User host = ucs.Where(uc1 => (uc1.User.Country == destCountry && uc1.User.City == destCity)).First().User; //might need to userid and .Find

                double fundsToTransfer = Convert.ToDouble(TotalCost(cycle.CycleId, user.ApartmentPrice).Data);
                TransferFunds(user, host, fundsToTransfer);

                RequestHandler.Instance.DeleteRequest(request, true); //mark request as complete
            }

            mutex.ReleaseMutex(); //release lock
            Session["HasActiveRequest"] = false;
            //added locking to this func, LockOut, DeleteRequest in userscontroller


            return Json(1, JsonRequestBehavior.AllowGet);
        }

        //move funds from user1 to user2
        private void TransferFunds(User u1, User u2, double funds)
        {
            u1.Balance -= funds;
            u2.Balance += funds;
            db.Entry(u1).Property("Balance").IsModified = true;
            db.Entry(u2).Property("Balance").IsModified = true;
            db.SaveChanges();
        }

        //calculates total cost by date range and price per night
        public JsonResult TotalCost(int? cid, double ppn)
        {
            Cycle cycle = db.Cycles.Find(cid);
            if (cycle == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            double totalCost = ((cycle.End - cycle.Start).TotalDays+1) * ppn;
            return Json(totalCost.ToString(), JsonRequestBehavior.AllowGet);
        }

        //checks if all usercycles in cycle are locked in
        private bool IsCycleComplete(Cycle cycle)
        {
            List<UserCycle> ucs = db.UserCycles.Where(uc => uc.CycleId == cycle.CycleId).ToList();
            foreach (UserCycle uc in ucs)
            {
                if (!uc.IsLocked)
                    return false;
            }
            return true;
        }

        //set session's LockedInCycleID to cid or empty string
        public JsonResult SetSessionLock(string cid)
        {
            Session["LockedInCycleID"] = cid;
            return Json(1,JsonRequestBehavior.AllowGet);
        }

    }
}
