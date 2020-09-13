using System;
using System.Collections.Generic;

namespace RailwayService.Core.Domain
{
    public class RailwayConnectionsGraph
    {
        public RailwayConnectionsGraph() { }
        public RailwayConnectionsGraph(IEnumerable<string> vertices, IEnumerable<Tuple<string, string, int>> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<string, HashSet<(string, int)>> AdjacencyList { get; } = new Dictionary<string, HashSet<(string, int)>>();

        public void AddVertex(string vertex)
        {
            AdjacencyList[vertex] = new HashSet<(string, int)>();
        }

        public void AddEdge(Tuple<string, string, int> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add((edge.Item2, edge.Item3));
                AdjacencyList[edge.Item2].Add((edge.Item1, edge.Item3));
            }
        }
    }
}
