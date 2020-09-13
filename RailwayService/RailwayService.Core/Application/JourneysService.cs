using RailwayService.Core.Domain;
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

            var shortestPath = CalculatePath(railwayConnectionsGraph, departFrom, arriveAt);

            if (shortestPath == null) return null;

            return new Journey() { DepartFrom = departFrom, ArriveAt = arriveAt, Time = shortestPath.Sum(x => x.Item2) };
        }

        private IEnumerable<(string, int)> CalculatePath(RailwayConnectionsGraph graph, string start, string end)
        {
            var previous = new Dictionary<string, (string, int)>();

            var queue = new Queue<(string, int)>();
            queue.Enqueue((start, 0));

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
       
            var path = new List<(string, int)> { };

            var current = (end, 0);
            while (!current.Item1.Equals(start))
            {
                path.Add(current);
                current = previous[current.Item1];
            };

            path.Add((start, current.Item2));
            path.Reverse();

            return path;
        }
    }
}
