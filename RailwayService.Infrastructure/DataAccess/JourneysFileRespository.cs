using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RailwayService.Core.Application;
using RailwayService.Core.Domain;
using System.Text.Json;
using System.Linq;

namespace RailwayService.Infrastructure.DataAccess
{
    public class JourneysFileRespository : IJourneysRespository
    {
        /* 
         * This repository loads destinations from a json file
         * In the future this could be replaced by a database, I used the interface to represent a common contract between repositories
         * When the implementation changes to a database, the only thing that needs to be done is to add a new repository that
         * uses the same interface but reads from a database instead (the signatures have been made async for that purpose).
        */
        private readonly ILogger<JourneysFileRespository> logger;
        private readonly List<Journey> collection;

        public JourneysFileRespository(ILogger<JourneysFileRespository> logger)
        {
            this.logger = logger;
            collection = LoadFromJson(Path.Combine("..", "RailwayService.Infrastructure", "RailwayStations.json"));
            

            if (collection == null)
            {
                var exception = new ArgumentNullException(nameof(collection));
                this.logger.LogError(exception.Message, exception);

                throw exception;
            }
        }

        public async Task<Journey> GetJourney(string departFrom, string arriveAt)
        {
            var result = collection
                .FirstOrDefault(x => 
                    x.DepartFrom.ToLowerInvariant() == departFrom.ToLowerInvariant() && 
                    x.ArriveAt.ToLowerInvariant() == arriveAt.ToLowerInvariant()
                );

            return await Task.FromResult(result);
        }

        public async Task<bool> AreValidLocations(string departFrom, string arriveAt) => await Task.FromResult(collection.Any(x => x.ArriveAt == departFrom || x.DepartFrom == arriveAt));

        public async Task<RailwayConnectionsGraph> GetAllAsRailwayConnectionsGraph()
        {
            var vertices = collection.Select(x => x.DepartFrom).Distinct();
            var edges = collection.Select(x => Tuple.Create(x.ArriveAt, x.DepartFrom, x.Time));

            return await Task.FromResult(new RailwayConnectionsGraph(vertices, edges));
        }

        private List<Journey> LoadFromJson(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<List<Journey>>(json);
            }
        }
    }
}