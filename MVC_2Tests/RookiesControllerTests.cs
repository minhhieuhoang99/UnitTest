using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MVC_2.Controllers;
using MVC_2.Models;
using MVC_2.Services;
using NUnit.Framework;

namespace MVC_2Tests
{
    public class RookiesControllerTests
    {

        private static PersonModel[] _persons = new PersonModel[]
        {
            new PersonModel
            {
                id=1,
                firstName="hieu1",
                lastName="hoang1",
                dateOfBirth = new DateTime (2000,08,09),
                gender="nam",
                phoneNumber=123456789,
                birthPlace="HN",
                age=10,
                isGraduated=true,
                email="hieu@gmai.com"
            },
            new PersonModel
            {
                id=2,
                firstName="hieu2",
                lastName="hoang2",
                dateOfBirth = new DateTime (2000,08,09),
                gender="nu",
                phoneNumber=123456789,
                birthPlace="HCM",age=10,
                isGraduated=true,
                email="hieu@gmai.com"
            },
            new PersonModel
            {
                id=3,
                firstName="hieu3",
                lastName="hoang3",
                dateOfBirth = new DateTime (2000,08,09),
                gender="nam",
                phoneNumber=123456789,
                birthPlace="QN",
                age=10,
                isGraduated=true,
                email="hieu@gmai.com"
            },
        };
        private ILogger<RookiesController> _logger;
        private Mock<IService> _service;
        [SetUp]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<RookiesController>>();

            _service = new Mock<IService>();
            _service.Setup(service => service.GetAll()).Returns(_persons);
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
            Assert.IsAssignableFrom<PersonModel[]>(((ViewResult)result).ViewData.Model);
            Assert.AreEqual(3, ((PersonModel[])((ViewResult)result).ViewData.Model).Count());
        }
        [Test]
        public void Detail_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            const int personId = 9999;
            _service.Setup(service => service.Get(personId)).Returns((PersonModel)null);
            var controller = new RookiesController(_logger, _service.Object);

            // Act
            var result = controller.Details(personId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void Detail_ReturnsAPerson()
        {
            // Arrange
            const int personId = 1;
            _service.Setup(service => service.Get(personId)).Returns(_persons.First());
            var controller = new RookiesController(_logger, _service.Object);
            const string expectedLastName = "hoang1";

            // Act
            var result = controller.Details(personId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsAssignableFrom<PersonModel>(((ViewResult)result).ViewData.Model);
            Assert.AreEqual(expectedLastName, ((PersonModel)((ViewResult)result).ViewData.Model).lastName);
        }
        [Test]
        public void Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            const string message = "some error";
            var controller = new RookiesController(_logger, _service.Object);
            controller.ModelState.AddModelError("error", message);

            // Act
            var result = controller.Create(person: null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.IsAssignableFrom<SerializableError>(((BadRequestObjectResult)result).Value);

            var error = (SerializableError)((BadRequestObjectResult)result).Value;
            Assert.AreEqual(1, error.Count);

            error.TryGetValue("error", out var msg);
            Assert.IsNotNull(msg);
            Assert.AreEqual(message, ((string[])msg).First());
        }
        [Test]
        public void Create_ReturnAPerson()
        {
            // Arrange
            int testId = 4;
            string testFirstName = "hieu4";
            string testLastName = "hoang4";
            DateTime testDateOfBirth = new DateTime(2000, 08, 09);
            string testGender = "nu";
            int testPhoneNumber = 123456789;
            string testBirthPlace = "HCM";
            int testAge = 10;
            bool testIsGraduated = true;
            string testEmail = "hieu@gmai.com";
            var testPerson = new PersonModel()
            {
                id = testId,
                firstName = testFirstName,
                lastName = testLastName,
                dateOfBirth = new DateTime(2000, 08, 09),
                gender = testGender,
                phoneNumber = testPhoneNumber,
                birthPlace = testBirthPlace,
                age = testAge,
                isGraduated = testIsGraduated,
                email = testEmail
            };
            var controller = new RookiesController(_logger, _service.Object);

            // Act
            var result = controller.Create(testPerson);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsAssignableFrom<PersonModel>(((ViewResult)result).ViewData.Model);
            // Assert.AreEqual(testPerson.lastName, ((PersonModel)((ViewResult)result).ViewData.Model).lastName);
            // Assert.AreEqual(testPerson.firstName, ((PersonModel)((ViewResult)result).ViewData.Model).firstName);
            // Assert.AreEqual(testPerson.dateOfBirth, ((PersonModel)((ViewResult)result).ViewData.Model).dateOfBirth);
            Assert.AreEqual(testPerson, ((PersonModel)((ViewResult)result).ViewData.Model));
        }
        [Test]
        public void Edit_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            const int personId = 9999;
            _service.Setup(service => service.Get(personId)).Returns((PersonModel)null);
            var controller = new RookiesController(_logger, _service.Object);

            // Act
            var result = controller.Edit(personId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);

        }
        [Test]
        public void Edit_ReturnsAPerson()
        {
            // Arrange
            const int personId = 2;
            _service.Setup(service => service.Get(personId)).Returns(_persons.FirstOrDefault(p => p.id == personId));
            var controller = new RookiesController(_logger, _service.Object);
            const string expectedLastName = "hoang2";

            // Act
            var result = controller.Edit(personId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsAssignableFrom<PersonModel>(((ViewResult)result).ViewData.Model);
            Assert.AreEqual(expectedLastName, ((PersonModel)((ViewResult)result).ViewData.Model).lastName);
        }
        [Test]
        public void Delete_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            const int personId = 9999;
            _service.Setup(service => service.Get(personId)).Returns((PersonModel)null);
            var controller = new RookiesController(_logger, _service.Object);

            // Act
            var result = controller.Delete(personId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void Delete_ReturnsAPerson()
        {
            // Arrange
            const int personId = 1;
            _service.Setup(service => service.Get(personId)).Returns(_persons.FirstOrDefault(p => p.id == personId));
            var controller = new RookiesController(_logger, _service.Object);
            const string expectedFirstName = "hieu1";

            // Act
            var result = controller.Delete(personId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsAssignableFrom<PersonModel>(((ViewResult)result).ViewData.Model);
            Assert.AreEqual(expectedFirstName, ((PersonModel)((ViewResult)result).ViewData.Model).firstName);
        }

    }
}