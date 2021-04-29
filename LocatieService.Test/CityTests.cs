using LocatieService.Controllers;
using LocatieService.Database.Converters;
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
    public class Tests
    {
        private Mock<ICityService> serviceMock;

        private City city;
        private CityRequest request;

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
            request = new CityRequest
            {
                Name = "Test"
            };
        }

        [Test]
        public async Task AddCityTest()
        {
            // Arrange
            serviceMock.Setup(x => x.AddAsync(request)).Returns(Task.FromResult(city));
            var controller = new CityController(serviceMock.Object);

            // Act
            var result = await controller.AddCity(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<City>>(result);
        }

        [Test]
        public async Task GetCityByIdTest()
        {
            // Arrange
            serviceMock.Setup(x => x.GetByIdAsync(new Guid())).Returns(Task.FromResult(city));
            var controller = new CityController(serviceMock.Object);

            // Act
            var result = await controller.GetCityById(new Guid());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<City>>(result);
        }
    }
}