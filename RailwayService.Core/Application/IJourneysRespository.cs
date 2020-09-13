using RailwayService.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RailwayService.Core.Application
{
    public interface IJourneysRespository
    {
        Task<Journey> GetJourney(string departFrom, string arriveAt);
        Task<List<Journey>> GetAll();
    }
}