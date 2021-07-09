using D6.Controllers;
using D6.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D6Tests
{
    public class RookiesControllerTests
    {
        private static List<Person> _members = new List<Person>
        {
            new Person{
                Id = 1,
                FirstName = "Bui",
                LastName = "Hung",
                Gender = "Male",
                DateOfBirth = new DateTime(1990,1,1),
                PhoneNumber="",
                BirthPlace = "Bac Ninh"
            },
            new Person{
                Id = 2,
                FirstName = "Tuan",
                LastName = "Mr.",
                Gender = "Male",
                DateOfBirth = new DateTime(1990,7,1),
                PhoneNumber="",
                BirthPlace = "Ha noi"
            }
        };
        private ILogger<RookiesController> _logger;
        private Mock<IPersonService> _service;

        [SetUp]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<RookiesController>>();

            _service = new Mock<IPersonService>();
            _service.Setup(service => service.GetAll()).Returns(_members);
        }

        [Test]
        public void Index_ReturnsAViewResult_WithAListOfPerson()
        {
            // Arrange
            var controller = new RookiesController(_logger, _service.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsAssignableFrom<List<Person>>(((ViewResult)result).ViewData.Model);
            Assert.AreEqual(2, ((List<Person>)((ViewResult)result).ViewData.Model).Count);
        }

        [Test]
        public void Detail_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            const int personId = 9999;
            _service.Setup(service => service.GetOne(personId)).Returns((Person)null);
            var controller = new RookiesController(_logger, _service.Object);

            // Act
            var result = controller.Detail(personId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Detail_ReturnsAPerson()
        {
            // Arrange
            const int personId = 1;
            _service.Setup(service => service.GetOne(personId)).Returns(_members.First());
            var controller = new RookiesController(_logger, _service.Object);
            const string expectedFullName = "Hung Bui";

            // Act
            var result = controller.Detail(personId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsAssignableFrom<Person>(((ViewResult)result).ViewData.Model);
            Assert.AreEqual(expectedFullName, ((Person)((ViewResult)result).ViewData.Model).FullName);
        }

        [Test]
        public void Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            const string message = "some error";
            var controller = new RookiesController(_logger, _service.Object);
            controller.ModelState.AddModelError("error", message);

            // Act
            var result = controller.Create(model: null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.IsAssignableFrom<SerializableError>(((BadRequestObjectResult)result).Value);

            var error = (SerializableError)((BadRequestObjectResult)result).Value;
            Assert.AreEqual(1, error.Count);

            error.TryGetValue("error", out var msg);
            Assert.IsNotNull(msg);
            Assert.AreEqual(message, ((string[])msg).First());
        }
    }
}