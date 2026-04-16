using System;
using System.Threading.Tasks;
using Bunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SchoolTimeCalc.Components.Pages;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;
using SchoolTimeCalc.Services;
using Xunit;

namespace SchoolTimeCalc.Tests
{
    public class SettingsTests : TestContext
    {
        [Fact]
        public void Settings_DisplaysLastHolidaySyncAndCount()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SettingsTestDb")
                .Options;
            
            var dbContext = new ApplicationDbContext(options);
            
            // Clean up from previous tests
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var user = new ApplicationUser 
            { 
                Id = 1, 
                Username = "testuser", 
                Bundesland = "Wien",
                WebUntisData = new WebUntisData 
                { 
                    SchoolName = "TestSchool", 
                    LastHolidaySync = new DateTime(2025, 1, 1, 12, 0, 0)
                } 
            };

            dbContext.Users.Add(user);
            
            dbContext.Holidays.Add(new Holiday { Name = "WinterBreak", SchoolId = "TestSchool", StartDate = DateTime.Now, EndDate = DateTime.Now });
            dbContext.Holidays.Add(new Holiday { Name = "SpringBreak", SchoolId = "TestSchool", StartDate = DateTime.Now, EndDate = DateTime.Now });
            
            dbContext.SaveChanges();

            Services.AddSingleton(dbContext);
            
            var authService = new MockAuthService(dbContext);
            Services.AddSingleton(authService);
            
            var syncServiceMock = new Mock<IHolidaySyncService>();
            Services.AddSingleton(syncServiceMock.Object);

            // Act
            var cut = RenderComponent<Settings>();

            // Assert
            Assert.Contains("Last synced on 1/1/2025", cut.Markup);
            Assert.Contains("2 holidays cached", cut.Markup);
        }

        [Fact]
        public void Settings_SyncButton_InvokesSyncService()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SettingsTestDb2")
                .Options;
            
            var dbContext = new ApplicationDbContext(options);
            
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var user = new ApplicationUser { Id = 1, Username = "testuser" };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            Services.AddSingleton(dbContext);
            
            var authService = new MockAuthService(dbContext);
            Services.AddSingleton(authService);
            
            var syncServiceMock = new Mock<IHolidaySyncService>();
            Services.AddSingleton(syncServiceMock.Object);

            var cut = RenderComponent<Settings>();

            // Act
            cut.Find("#server").Change("tipo.webuntis.com");
            cut.Find("#school").Change("TestSchool");
            cut.Find("#username").Change("user");
            cut.Find("#password").Change("pass");

            var syncButton = cut.Find("button:contains('Sync Holidays Now')");
            syncButton.Click();

            // Assert
            syncServiceMock.Verify(s => s.SyncHolidaysAsync("tipo.webuntis.com", "TestSchool", "user", "pass", default), Times.Once);
            Assert.Contains("Holidays synced successfully!", cut.Markup);
        }
    }
}
