using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using Todo.Controllers;
using Todo.Models;
using Xunit;

namespace MyApp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Index_ReturnsRedirect_WhenUsernameIsNull()
        {
            // Arrange 
            var mockSession = new Mock<ISession>();
            byte[] dummy;
            mockSession.Setup(s => s.TryGetValue("Username", out dummy)).Returns(false);

            var context = new DefaultHttpContext();
            context.Session = mockSession.Object;

            var controller = new HomeController(new Mock<ILogger<HomeController>>().Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = context
                }
            };

            // Act 
            var result = controller.Index();

            // Assert 
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirect.ActionName);
        }

        [Fact]
        public void Index_ReturnsView_WhenUsernameExists()
        {
            // Arrange 
            string username = "testUser";
            var mockSession = new Mock<ISession>();

            var usernameBytes = Encoding.UTF8.GetBytes(username);
            byte[] outBytes = null;

            mockSession
                .Setup(s => s.TryGetValue("Username", out outBytes))
                .Callback(new TryGetValueCallback((string key, out byte[] val) => val = usernameBytes))
                .Returns(true);

            var context = new DefaultHttpContext();
            context.Session = mockSession.Object;

            var controller = new HomeController(new Mock<ILogger<HomeController>>().Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = context
                }
            };

            // Act 
            var result = controller.Index();

            // Assert 
            Assert.IsType<ViewResult>(result);
        }

        private delegate void TryGetValueCallback(string key, out byte[] value);

        [Fact]
        public void Insert_AddsTodoItem_WhenUserIsLoggedIn()
        {
            // Arrange
            var mockSession = new Mock<ISession>();
            var username = "testUser";
            var usernameBytes = Encoding.UTF8.GetBytes(username);
            byte[] outBytes = null;

            mockSession
                .Setup(s => s.TryGetValue("Username", out outBytes))
                .Callback(new TryGetValueCallback((string key, out byte[] val) => val = usernameBytes))
                .Returns(true);

            var context = new DefaultHttpContext();
            context.Session = mockSession.Object;

            var controller = new HomeController(new Mock<ILogger<HomeController>>().Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = context
                }
            };

            var todoItem = new TodoItem { Name = "Test Todo" };

            // Act
            var result = controller.Insert(todoItem);

            // Assert
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void Update_UpdatesTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Name = "Updated Todo" };
            var controller = new HomeController(new Mock<ILogger<HomeController>>().Object);

            // Act
            var result = controller.Update(todoItem);

            // Assert
            Assert.IsType<RedirectResult>(result);
        }
        [Fact]
        public void Delete_DeletesTodoItem()
        {
            // Arrange
            var controller = new HomeController(new Mock<ILogger<HomeController>>().Object);
            int idToDelete = 1;

            // Act
            var result = controller.Delete(idToDelete);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult.Value);
        }
        [Fact]
        public void Register_CreatesUser_WhenNotExists()
        {
            // Arrange
            var user = new User { Username = "newUser", Password = "password123" };
            var controller = new HomeController(new Mock<ILogger<HomeController>>().Object);

            // Act
            var result = controller.Register(user);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
        }


    }
}