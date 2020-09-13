using Moq;
using RailwayService.Core.Application;
using RailwayService.Core.Domain;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RailwayService.Tests
{
    public class JourneysServiceTests
    {
        private readonly Mock<IJourneysRespository> journeysRepository;
        private readonly JourneysService service;

        public JourneysServiceTests()
        {
            this.journeysRepository = new Mock<IJourneysRespository>();
            this.service = new JourneysService(journeysRepository.Object);
        }

        [Fact]
        public async Task IfItDidntFoundAMatchingJourney_GetJourney_ShoulReturnNull()
        {
            var result = await service.GetJourney("London", "Antwerp");

            Assert.Null(result);
        }

        [Fact]
        public async Task IfEverythingIsOk_GetJourney_ShouldReturnTheExpectedJourney()
        {
            var journey = new Journey { DepartFrom = "Basingstoke", ArriveAt = "Reading", Time = 18 };
            journeysRepository.Setup(x => x.GetJourney(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(journey);

            var result = await service.GetJourney("Basingstoke", "Reading");

            Assert.Equal(journey, result);
        }

        [Fact]
        public async Task IfEverythingIsOk_GetJourney_ShouldReturnTheExpectedJourneyWithIntermediateConnections()
        {
            var expectedJourney = new Journey { DepartFrom = "Basingstoke", ArriveAt = "London Victoria", Time = 43 };

            var result = await service.GetJourney(expectedJourney.DepartFrom, expectedJourney.ArriveAt);

            Assert.Equal(expectedJourney, result);
        }
    }
}
