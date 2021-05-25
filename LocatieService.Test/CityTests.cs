using LocatieService.Controllers;
using LocatieService.Database.Datamodels;
using LocatieService.Database.Datamodels.Dtos;
using LocatieService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LocatieService.Test
{
    [TestFixture]
    public class CityTests
    {
        private Mock<ICityService> serviceMock;

        private City city;
        private CityRequest cityRequest;
        private CityResponse cityResponse;

        [SetUp]
        public void Setup()
        {
            // Instantiate mocks:
            serviceMock = new Mock<ICityService>();

            // Create mock data:
            city = new City
            {
                Name = "Test"
            };
            
            cityRequest = new CityRequest
            {
                Name = "Test"
            };

            cityResponse = new CityResponse
            {
                Name = "Test"
            };
        }

        [Test]
        public async Task AddCityTest()
        {
            // Arrange
            serviceMock.Setup(x => x.AddAsync(cityRequest)).Returns(Task.FromResult(cityResponse));
            var controller = new CityController(serviceMock.Object);

            // Act
            var result = await controller.AddCity(cityRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<CityResponse>>(result);
        }

        [Test]
        public async Task GetCityByIdTest()
        {
            // Arrange
            serviceMock.Setup(x => x.GetByIdAsync(new Guid())).Returns(Task.FromResult(cityResponse));
            var controller = new CityController(serviceMock.Object);

            // Act
            var result = await controller.GetCityById(new Guid());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<CityResponse>>(result);
        }
    }
}