using swop.Graphs;
using swop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;


namespace swop.Requests
{
    class RequestHandler
    {
        const int SUB_DAYS = -2;
        const int ADD_DAYS = 2;
        //lazy thread-safe singleton
        private static readonly Lazy<RequestHandler> Lazy = new Lazy<RequestHandler> (() => new RequestHandler());
        public static RequestHandler Instance { get { return Lazy.Value; } }
        private SwopContext db = new SwopContext();

        protected Dictionary<string, Graph> _graphs;

      

        private RequestHandler()
        {
            _graphs = new Dictionary<string, Graph>();
        }
 
        
        //find all relevant requests and add to memory
        public void AddRequest(Request request, bool updateDb)
        {
            List<Request> reqList = Get_All_CoRequests(request);
            //Add the request to all applicable dates
            foreach (Request r in reqList)
            {
                GetGraph(Get_DateRange_String(r)).AddVertex(r.User.UserId.ToString(), r.From, r.To);
                if (updateDb)
                {
                    addRequestToDb(r);
                }
            }
        }

        //put request in db
        private void addRequestToDb(Request r)
        {
            List<List<string>> cycles = filterCyclesForUser(FindCycles(r.Start, r.End), r);
            foreach (List<string> cycle in cycles)
            {
                Cycle c = new Cycle()
                {
                    Start = r.Start,
                    End = r.End
                };
                db.Cycles.Add(c);
                //save changes so added cycle gets an id? if so need to re-update the c object so it'll have its id
                List<UserCycle> userCycles = new List<UserCycle>(); //list to save in cycle
                foreach (string userId in cycle) //add usercycle for every user in cycle
                {
                    UserCycle uc = new UserCycle()
                    {
                        UserId = Int32.Parse(userId),
                        User = db.Users.Find(userId), //needed?
                        CycleId = c.CycleId,
                        Cycle = db.Cycles.Find(c.CycleId), //needed?
                        IsLocked = false
                    };
                    db.UserCycles.Add(uc);
                    userCycles.Add(uc);
                }
                c.UserCycles = userCycles; //update cycle's usercycles
                db.Entry(c).Property("UserCycles").IsModified = true; 
            }
            db.Requests.Add(r);
            db.SaveChanges();
            /*
             for each cycle in cycles create the relevant cycle model and usercycles and update the db
             after foreach add request to db
             remember to saveChanges()
             * */

        }

        //filter cycles for user in request parameter
        private List<List<string>> filterCyclesForUser(List<List<string>> cycles, Request r)
        {
            string userIdS = r.User.UserId.ToString();
            return (from cycle in cycles where cycle.Contains(userIdS) select cycle).ToList();
        }

        public void DeleteRequest(Request request) //because of success or failure? add boolean to determine for request state
        {
            List<Request> reqList = Get_All_CoRequests(request);
            foreach (Request r in reqList)
            {
                GetGraph(Get_DateRange_String(r)).DeleteVertex(r.User.UserId.ToString(), r.From, r.To);
            }
            List<Cycle> cycles = new List<Cycle>();
            int userId = request.UserId;
            foreach (UserCycle uc in db.UserCycles)
            {
                if (uc.UserId == userId)
                {
                    cycles.Add(uc.Cycle);
                }
            }
            foreach (Cycle cycle in cycles)
            {
                foreach (UserCycle uc in cycle.UserCycles) //removing while iterating? ok because its db?
                {
                    db.UserCycles.Remove(uc);
                }
                db.Cycles.Remove(cycle);
            }
            request.State = 2; //1 completed 2 failed
            db.Entry(request).Property("State").IsModified = true;
            db.SaveChanges();
            //delete every cycle that contains the user
            //request's user -> go through all usercycles, save all cycles to a list.
            //go through cycle list -> delete all user cycles inside, and then delete the cycle
            //in the end - delete request (or change request state)
            //remember to savechanges
        }

        public List<List<string>> FindCycles(DateTime s, DateTime e)
        {
            return GetGraph(Get_DateRange_String(s, e)).findCycles();
        }
        

        protected Graph GetGraph(string id)
        {
            if (!_graphs.TryGetValue(id, out Graph g))
            {
                g = new Graph();
                _graphs.Add(id, g);
            }
            return g;
        }
    

        //finds all relative requests in a range of 2 days
        public List<Request> Get_All_CoRequests(Request req)
        {
            List<Request> reqList = new List<Request>();
            for (int i = SUB_DAYS; i <= ADD_DAYS; i++)
            {
                for (int j = SUB_DAYS; j <= ADD_DAYS; j++)
                {
                    DateTime s = req.Start.AddDays(i);
                    DateTime e = req.End.AddDays(j);
                    if (DateTime.Compare(s, e) <= 0 && DateTime.Compare(s, DateTime.Today) >= 0)
                        reqList.Add(Copy_Request_With_New_Date(req, s, e));
                }
            }
            return reqList;
        }


        public Request Copy_Request_With_New_Date(Request req, DateTime s, DateTime e)
        {
            return new Request() {
                UserId = req.UserId,
                User = req.User,
                From = req.From,
                To = req.To,
                Start = s,
                End = e,
                State = req.State
            };
        }

        public string Get_DateRange_String(Request req)
        {
            return req.Start.ToString("dd/MM/yyyy") + "-" + req.End.ToString("dd/MM/yyyy");
        }

        public string Get_DateRange_String(DateTime s, DateTime e)
        {
            return s.ToString("dd/MM/yyyy") + "-" + e.ToString("dd/MM/yyyy");
        }

    }
}
