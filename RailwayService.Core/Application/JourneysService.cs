using RailwayService.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailwayService.Core.Application
{
    public class JourneysService : IJourneysService
    {
        private readonly IJourneysRespository journeysRespository;

        public JourneysService(IJourneysRespository journeysRespository)
        {
            this.journeysRespository = journeysRespository;
        }

        public async Task<Journey> GetJourney(string departFrom, string arriveAt)
        {
            if (!await journeysRespository.AreValidLocations(departFrom, arriveAt)) return null;

            var result = await journeysRespository.GetJourney(departFrom, arriveAt);

            if (result != null) return result;

            return await GetJourneyWithIntermediateConnections(departFrom, arriveAt);
        }

        private async Task<Journey> GetJourneyWithIntermediateConnections(string departFrom, string arriveAt)
        {
            var railwayConnectionsGraph = await journeysRespository.GetAllAsRailwayConnectionsGraph();

            var CalculateShortestPath = ShortestPathFunction(railwayConnectionsGraph, (departFrom, 0));
            var shortestPath = CalculateShortestPath((arriveAt, 0));

            return new Journey() { DepartFrom = departFrom, ArriveAt = arriveAt, Time = shortestPath.Sum(x => x.Item2) };
        }

        public Func<(string, int), IEnumerable<(string, int)>> ShortestPathFunction(RailwayConnectionsGraph graph, (string,int) start)
        {
            var previous = new Dictionary<string, (string, int)>();

            var queue = new Queue<(string, int)>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex.Item1])
                {
                    if (previous.ContainsKey(neighbor.Item1))
                        continue;

                    previous[neighbor.Item1] = (vertex.Item1, neighbor.Item2);
                    queue.Enqueue(neighbor);
                }
            }

            Func<(string, int), IEnumerable<(string, int)>> shortestPath = v => {
                var path = new List<(string, int)> { };

                var current = v;
                while (!current.Item1.Equals(start.Item1))
                {
                    path.Add(current);
                    current = previous[current.Item1];
                };

                path.Add((start.Item1, current.Item2));
                path.Reverse();

                return path;
            };

            return shortestPath;
        }
    }
}
