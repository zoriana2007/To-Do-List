using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Logging; 
using Moq; 
using System.Text; 
using Todo.Controllers; 
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
    } 
}