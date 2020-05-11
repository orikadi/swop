using swop.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swop.Requests
{
    class RequestHandler
    {
        protected Dictionary<string, Graph> _graphs;

        public RequestHandler()
        {
            _graphs = new Dictionary<string, Graph>();
        }
 
        public void AddRequest(Request request)
        {
            //Add the request to all aplicable dates
            foreach (DateRange r in request.GetDateRange().GenerateAllRanges())
            {
                GetGraph(r.ToString()).AddVertex(request.GetId(), request.GetFrom(), request.GetTo());
            }
        }

        public void DeleteRequest(Request request)
        {
            foreach(DateRange r in request.GetDateRange().GenerateAllRanges())
            {
                GetGraph(r.ToString()).DeleteVertex(request.GetId(),request.GetFrom(), request.GetTo());
            }
        }

        public List<List<string>> FindCycles(DateRange dateRange)
        {
            return GetGraph(dateRange.ToString()).findCycles();
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
    }
}
