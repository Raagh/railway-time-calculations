using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RailwayService.Api.Model;
using RailwayService.Core.Application;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace RailwayService.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class JourneysController : ControllerBase
    {
        private readonly IJourneysService journeysService;
        private readonly IMapper mapper;

        public JourneysController(IJourneysService journeysService, IMapper mapper)
        {
            this.journeysService = journeysService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves a railway journey between two destinations
        /// </summary>
        /// <returns>a journey model</returns>
        [HttpGet("{from}/{to}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Journey>> Get(string from, string to)
        {
            var result = await journeysService.GetJourney(from, to);

            if (result == null) return NotFound();

            var response = mapper.Map<Core.Domain.Journey, Journey>(result);

            return Ok(response);
        }
    }
}
