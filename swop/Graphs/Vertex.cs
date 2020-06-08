using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swop.Graphs
{
    class Vertex
    {
        public string Id{get; private set;}
        public Dictionary<Vertex,List<string>> Edges {get;private set;}

        public Vertex(string id)
        {
            Id=id;
            Edges=new Dictionary<Vertex, List<string>>();
        }

        public void AddEdge(string user,Vertex to)
        {
            if(Edges.ContainsKey(to))
            {
                if (!Edges[to].Contains(user))
                {
                    Edges[to].Add(user);
                }
            }    
            else
            {
                Edges.Add(to,new List<string>());
                Edges[to].Add(user);
            }
        }

        public void RemoveEdge(string user,Vertex to)
        {
            Edges[to].Remove(user);
            if(!Edges[to].Any())
                Edges.Remove(to);
        }

        public List<Vertex> GetNeighbours()
        {
            return new List<Vertex>(Edges.Keys);
        }

        public List<string> GetEdgeParticipants(Vertex v)
        {
            return Edges[v];
        }
    }
}
