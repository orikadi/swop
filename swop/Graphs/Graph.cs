using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swop.Graphs
{
    class Graph
    {
        Dictionary<string, Vertex> _vertexes;
        int lowLimit = 3;
        int highLimit = 6;

        public Graph()
        {
            _vertexes = new Dictionary<string, Vertex>();
        }

        public void AddVertex(string id, string fromId, string toId)
        {
            GetVertex(fromId).AddEdge(id, GetVertex(toId));
        }

        public void DeleteVertex(string id, string fromId, string toId)
        {
            GetVertex(fromId).RemoveEdge(id, GetVertex(toId));
        }

        public Vertex GetVertex(string id)
        {
            if (!_vertexes.TryGetValue(id, out Vertex v))
            {
                v = new Vertex(id);
                _vertexes.Add(id, v);
            }
            return v;
        }

        public List<List<string>> findCycles()
        {
            return GenerateParticipantCycles(FilterCycles(findAllCycles()));
        }

        private List<List<Vertex>> findAllCycles()
        {
            List<List<Vertex>> currentLevel = new List<List<Vertex>>();
            List<List<Vertex>> nextLevel = new List<List<Vertex>>();
            List<List<Vertex>> cycles = new List<List<Vertex>>();
            foreach (KeyValuePair<string, Vertex> entry in _vertexes) //Generate seed from all the vertexes in the graph
            {
                List<Vertex> temp = new List<Vertex>();
                temp.Add(entry.Value);
                currentLevel.Add(temp);
            }
            for (int i = 1; i < lowLimit; i++) //Generate start of cycles to the min length
            {
                foreach (List<Vertex> currentList in currentLevel)
                {
                    List<Vertex> neighbours = currentList.Last().GetNeighbours();
                    //For each neighbour of the last element in the lsit create a new list and add the neighbour
                    foreach (Vertex neighbur in neighbours)
                    {
                        List<Vertex> temp = new List<Vertex>(currentList);
                        temp.Add(neighbur);
                        nextLevel.Add(temp);
                    }
                }
                currentLevel = nextLevel; //Now what we created would become the current level, and we add upon it
                nextLevel = new List<List<Vertex>>();
            }

            for (int i = lowLimit; i <= highLimit; i++) //Closing cycles
            {
                foreach (List<Vertex> currentList in currentLevel)
                {
                    List<Vertex> neighbours = currentList.Last().GetNeighbours();
                    foreach (Vertex neighbour in neighbours)
                    {
                        List<Vertex> temp = new List<Vertex>(currentList);
                        if (!temp.First().Equals(neighbour)) //Check if the cycle is closed
                        {
                            //If not, add the neighbour and add it to the next level
                            temp.Add(neighbour);
                            nextLevel.Add(temp);
                        }
                        else
                        {
                            //If yes, add it to the cycle list (and don't add the neighbour to the list (so we save space))
                            cycles.Add(temp);
                        }
                    }
                }
                currentLevel = nextLevel;
                nextLevel = new List<List<Vertex>>();
            }
            return cycles;
        }

        private List<List<Vertex>> FilterCycles(List<List<Vertex>> cycles)
        {
            Dictionary<string, List<Vertex>> dict = new Dictionary<string, List<Vertex>>();
            for (int i = cycles.Count - 1; i >= 0; i--)
            {
                if (cycles[i].Count == cycles[i].Distinct().Count()) //Remove cycles if that has repeating items (for example: a,b,a,b,(a)
                {
                    //Save cycles in dictionary with key so we don't have same cycles in different order
                    string key = string.Join(",", RotateVertexCycle((from vertex in cycles[i] select vertex.Id).ToList()));
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, cycles[i]);
                    }
                }
            }
            return dict.Values.ToList();
        }

        private List<string> RotateVertexCycle(List<string> cycle)
        {
            int minI = 0;
            //Find the smallest element
            for (int i = 1; i < cycle.Count; i++)
            {
                if (cycle[minI].CompareTo(cycle[i]) > 0)
                    minI = i;
            }
            List<string> temp = new List<string>(cycle);
            //Rotate the list so it will be the first one
            for (int i = 0; i < minI; i++)
            {
                temp.Add(temp.FirstOrDefault());
                temp.RemoveAt(0);
            }
            return temp;
        }

        private List<List<string>> GenerateParticipantCycles(List<List<Vertex>> cycles)
        {
            List<List<string>> participantCycles = new List<List<string>>();
            foreach (List<Vertex> cycle in cycles)
                participantCycles.AddRange(ParticipantsFromVertexCycle(cycle));
            return participantCycles;
        }

        private List<List<string>> ParticipantsFromVertexCycle(List<Vertex> cycle)
        {
            //Get all participants in the cycle (from the edges)
            List<List<string>> participantsInCycle = new List<List<string>>();
            for (int i = 0; i < cycle.Count - 1; i++)
                participantsInCycle.Add(cycle[i].GetEdgeParticipants(cycle[i + 1]));
            participantsInCycle.Add(cycle[cycle.Count - 1].GetEdgeParticipants(cycle[0])); //get participants from edge from the last element to first element

            List<List<string>> currentLevel = GenerateSeed(participantsInCycle[0]);
            for (int i = 1; i < participantsInCycle.Count; i++)
            {
                currentLevel = CurrentToNextLevel(currentLevel, participantsInCycle[i]);
            }
            return currentLevel;
        }

        private List<List<string>> GenerateSeed(List<string> first)
        {
            List<List<string>> seed = new List<List<string>>();
            foreach (string s in first)
            {
                List<string> temp = new List<string>
                {
                    s
                };
                seed.Add(temp);
            }
            return seed;
        }

        private List<List<string>> CurrentToNextLevel(List<List<string>> currentLevel, List<string> edgeParticipants)
        {
            List<List<string>> nextLevel = new List<List<string>>();
            foreach (List<string> currentList in currentLevel)
            {
                foreach (string participant in edgeParticipants)
                {
                    List<string> temp = new List<string>(currentList)
                    {
                        participant
                    };
                    nextLevel.Add(temp);
                }
            }
            return nextLevel;
        }
    }
}
