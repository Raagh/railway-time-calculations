using System;
using System.Collections.Generic;
using System.Text;

namespace RailwayService.Core.Application
{
    public class Graph<T,E>
    {
        public Graph() { }
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T, E>> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<T, HashSet<(T, E)>> AdjacencyList { get; } = new Dictionary<T, HashSet<(T, E)>>();

        public void AddVertex(T vertex)
        {
            AdjacencyList[vertex] = new HashSet<(T, E)>();
        }

        public void AddEdge(Tuple<T, T, E> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add((edge.Item2, edge.Item3));
                AdjacencyList[edge.Item2].Add((edge.Item1, edge.Item3));
            }
        }
    }
}
