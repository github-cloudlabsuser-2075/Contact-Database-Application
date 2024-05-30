using NUnit.Framework;
using CRUD_application_2.Controllers;
using CRUD_application_2.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CRUD_application_2.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        private UserController _controller;
        private List<User> _userList;

        [SetUp]
        public void Setup()
        {
            // Initialize the UserController and the user list
            _controller = new UserController();
            _userList = new List<User>
            {
                new User { Id = 1, Name = "Test User 1", Email = "test1@example.com" },
                new User { Id = 2, Name = "Test User 2", Email = "test2@example.com" },
            };

            UserController.userlist = _userList;
        }

        [Test]
        public void Index_ReturnsFilteredUserList_WhenSearchStringIsProvided()
        {
            // Arrange
            string searchString = "Test User 1";

            // Act
            ViewResult result = _controller.Index(searchString) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<User>;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual(searchString, model[0].Name);
        }


        [Test]
        public void Index_ReturnsViewResult_WithUserList()
        {
            // Act
            ViewResult result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_userList, result.Model);
        }

        [Test]
        public void Details_ReturnsUser_WhenUserExists()
        {
            // Act
            ViewResult result = _controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            User user = result.Model as User;
            Assert.AreEqual(_userList[0], user);
        }

        [Test]
        public void Details_ReturnsHttpNotFound_WhenUserDoesNotExist()
        {
            // Act
            HttpNotFoundResult result = _controller.Details(999) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_AddsNewUser_WhenModelStateIsValid()
        {
            // Arrange
            User newUser = new User { Id = 3, Name = "Test User 3", Email = "test3@example.com" };

            // Act
            RedirectToRouteResult result = _controller.Create(newUser) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, UserController.userlist.Count);
            Assert.AreEqual(newUser, UserController.userlist.Last());
        }

        [Test]
        public void Edit_UpdatesUser_WhenUserExists()
        {
            // Arrange
            User updatedUser = new User { Id = 1, Name = "Updated User", Email = "updated@example.com" };

            // Act
            RedirectToRouteResult result = _controller.Edit(1, updatedUser) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUser.Name, UserController.userlist[0].Name);
            Assert.AreEqual(updatedUser.Email, UserController.userlist[0].Email);
        }

        [Test]
        public void Edit_ReturnsHttpNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            User updatedUser = new User { Id = 999, Name = "Updated User", Email = "updated@example.com" };

            // Act
            HttpNotFoundResult result = _controller.Edit(999, updatedUser) as HttpNotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
