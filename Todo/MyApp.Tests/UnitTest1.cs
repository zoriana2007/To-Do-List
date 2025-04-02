using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
namespace MyApp.Tests;
public class UnitTest1
{
    [Fact]
        public void WebApplication_Can_Build_And_Configure()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[] { });

            // Додаємо сервіси
            builder.Services.AddControllersWithViews();

            // Act
            var app = builder.Build();

            
            var isDevelopment = app.Environment.IsDevelopment(); 
            // Assert
            Assert.NotNull(app);
            Assert.False(isDevelopment); 
        }
}