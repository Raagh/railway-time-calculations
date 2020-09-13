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
            var result = await journeysRespository.GetJourney(departFrom, arriveAt);

            if (result != null) return result;

            return await GetJourneyWithIntermediateConnections(departFrom, arriveAt);
        }

        private async Task<Journey> GetJourneyWithIntermediateConnections(string departFrom, string arriveAt)
        {
            var connections = await journeysRespository.GetAll();

            var vertices = connections.Select(x => x.DepartFrom).Distinct();
            var edges = connections.Select(x => Tuple.Create(x.ArriveAt, x.DepartFrom, x.Time));

            var graph = new Graph<string, int>(vertices, edges);

            var CalculateShortestPath = ShortestPathFunction(graph, (departFrom, 0));

            var shortestPath = CalculateShortestPath((arriveAt, 0));

            return new Journey() { DepartFrom = departFrom, ArriveAt = arriveAt, Time = shortestPath.Sum(x => x.Item2) };
        }

        public Func<(T, int), IEnumerable<(T, int)>> ShortestPathFunction<T>(Graph<T, int> graph, (T,int) start)
        {
            var previous = new Dictionary<T, (T, int)>();

            var queue = new Queue<(T, int)>();
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

            Func<(T, int), IEnumerable<(T, int)>> shortestPath = v => {
                var path = new List<(T, int)> { };

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
