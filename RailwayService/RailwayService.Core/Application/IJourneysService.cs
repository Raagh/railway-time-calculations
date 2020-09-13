using RailwayService.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RailwayService.Core.Application
{
    public interface IJourneysService
    {
        Task<Journey> GetJourney(string departFrom, string arriveAt);
    }
}