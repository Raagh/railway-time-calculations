using Moq;
using RailwayService.Core.Application;
using RailwayService.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RailwayService.Tests
{
    public class JourneysServiceTests
    {
        private readonly Mock<IJourneysRespository> journeysRepository;
        private readonly JourneysService service;
        private readonly RailwayConnectionsGraph mockedGraph;

        public JourneysServiceTests()
        {
            this.journeysRepository = new Mock<IJourneysRespository>();
            this.service = new JourneysService(journeysRepository.Object);
            var vertices = new List<string>() { "Basingstoke", "Reading", "Farnborough", "Clapham Junction", "London Victoria", "London Paddington" };
            var edges = new List<Tuple<string, string, int>>() {
                Tuple.Create("Reading", "Basingstoke", 18),
                Tuple.Create("Basingstone", "Reading", 18),
                Tuple.Create("Farnborough", "Basingstoke", 16),
                Tuple.Create("Basingstoke", "Farnborough", 16),
                Tuple.Create("Farnborough", "Reading", 25),
                Tuple.Create("Reading", "Farnborough", 25),
                Tuple.Create("Clapham Junction", "Basingstoke", 37),
                Tuple.Create("Basingstoke", "Clapham Junction", 37),
                Tuple.Create("London Victoria", "Clapham Junction", 6),
                Tuple.Create("Clapham Junction", "London Victoria", 6),
                Tuple.Create("London Paddington", "Reading", 34),
                Tuple.Create("Reading", "London Paddington", 34),
                Tuple.Create("London Victoria", "London Paddington", 32),
                Tuple.Create("London Paddington", "London Victoria", 32)
            };
            this.mockedGraph = new RailwayConnectionsGraph(vertices, edges);
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
            journeysRepository.Setup(x => x.AreValidLocations(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var result = await service.GetJourney("Basingstoke", "Reading");

            Assert.Equal(journey, result);
        }

        [Theory]
        [InlineData("Basingstoke", "London Victoria", 43)]
        [InlineData("Reading", "London Victoria", 66)]
        [InlineData("Farnborough", "London Victoria", 59)]
        [InlineData("London Paddington", "London Victoria", 32)]
        [InlineData("Clampham Junction", "London Victoria", 6)]
        public async Task IfEverythingIsOk_GetJourney_ShouldReturnTheExpectedJourneyWithIntermediateConnections(string departFrom, string arriveAt, int time)
        {
            var expectedJourney = new Journey { DepartFrom = departFrom, ArriveAt = arriveAt, Time = time };
            journeysRepository.Setup(x => x.GetAllAsRailwayConnectionsGraph()).ReturnsAsync(mockedGraph);
            journeysRepository.Setup(x => x.AreValidLocations(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var result = await service.GetJourney(expectedJourney.DepartFrom, expectedJourney.ArriveAt);

            Assert.Equal(expectedJourney.DepartFrom, result.DepartFrom);
            Assert.Equal(expectedJourney.ArriveAt, result.ArriveAt);
            Assert.Equal(expectedJourney.Time, result.Time);
        }
    }
}
