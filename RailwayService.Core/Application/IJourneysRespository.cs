using RailwayService.Core.Domain;
using System.Threading.Tasks;

namespace RailwayService.Core.Application
{
    public interface IJourneysRespository
    {
        Task<Journey> GetJourney(string departFrom, string arriveAt);
        Task<RailwayConnectionsGraph> GetAllAsRailwayConnectionsGraph();
        Task<bool> AreValidLocations(string departFrom, string arriveAt);
    }
}