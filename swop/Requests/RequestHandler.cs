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
            List<Request> reqList;
            if (updateDb) //if request isnt new (is brought up from db) -> dont call corequests, so you dont get overpopulation (corequests of corequests of original request)
            {
                reqList = Get_All_CoRequests(request, true);
            }  
           else
           {
                reqList = new List<Request> { request };
           }
            //Add the request to all applicable dates
            foreach (Request r in reqList)
            {
                GetGraph(Get_DateRange_String(r)).AddVertex(r.UserId.ToString(), r.From, r.To); //changed r.user.userid to r.userid
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
                db.SaveChanges();
                //save changes so added cycle gets an id? if so need to re-update the c object so it'll have its id
                //List<UserCycle> userCycles = new List<UserCycle>(); //list to save in cycle
                foreach (string userId in cycle) //add usercycle for every user in cycle
                {
                    int userIdInt = Int32.Parse(userId);
                    UserCycle uc = new UserCycle()
                    {
                        UserId = userIdInt,
                        User = db.Users.Find(userIdInt), //needed?
                        CycleId = c.CycleId,
                        Cycle = db.Cycles.Find(c.CycleId), //needed?
                        IsLocked = false
                    };
                    db.UserCycles.Add(uc);
                   // db.SaveChanges();
                }
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
            string userIdS = r.UserId.ToString(); //changed r.user.userid to r.userid
            return (from cycle in cycles where cycle.Contains(userIdS) select cycle).ToList();
        }

        //deletes a request
        //delete every cycle that contains the user
        //request's user -> go through all usercycles, save all cycles to a list.
        //go through cycle list -> delete all user cycles inside, and then delete the cycle
        //change request state according to bool parameter
        public void DeleteRequest(Request request, bool didSucceed)
        {
            List<Request> reqList = new List<Request>(); //actual requests from db
            //get all requests from db that have the same userId and 'ongoing' state
            reqList = (from dbReq in db.Requests where dbReq.UserId == request.UserId && dbReq.State == 0 select dbReq).ToList();
            //delete corresponding vertexes from graphs
            foreach (Request r in reqList)
            {
                GetGraph(Get_DateRange_String(r)).DeleteVertex(r.UserId.ToString(), r.From, r.To);
            }
            List<Cycle> cycles = new List<Cycle>(); //might change to set
            int userId = request.UserId;
            foreach (UserCycle uc in db.UserCycles.Include("Cycle"))
            {
                if (uc.UserId == userId)
                {
                    cycles.Add(uc.Cycle);
                }
            }
            foreach (Cycle cycle in cycles)
            {
                foreach (UserCycle uc in cycle.UserCycles.ToList())
                {
                    db.UserCycles.Remove(uc);
                }
                db.Cycles.Remove(cycle);
            }
            foreach(Request r in reqList)
            {
                if (didSucceed && r.RequestId == request.RequestId) r.State = 1;
                else r.State = 2;
                db.Entry(r).Property("State").IsModified = true;
            }
            db.SaveChanges();
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
        //add boolean thats marks if we should consider today
        public List<Request> Get_All_CoRequests(Request req, bool compareToday)
        {
            List<Request> reqList = new List<Request>();
            for (int i = SUB_DAYS; i <= ADD_DAYS; i++)
            {
                for (int j = SUB_DAYS; j <= ADD_DAYS; j++)
                {
                    DateTime s = req.Start.AddDays(i);
                    DateTime e = req.End.AddDays(j);
                    if (DateTime.Compare(s, e) <= 0 && (!compareToday || DateTime.Compare(s, DateTime.Today) >= 0))
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
