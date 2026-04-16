using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Refit;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;
using SchoolTimeCalc.Services;
using Xunit;

namespace SchoolTimeCalc.Tests
{
    public class HolidaySyncServiceTests
    {
        [Fact]
        public async Task SyncHolidaysAsync_SavesHolidaysToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new ApplicationDbContext(options);
            
            // Add required user and webuntis data
            var user = new ApplicationUser { Id = 1, Username = "testuser", Bundesland = "Wien" };
            var untisData = new WebUntisData { Id = 1, ApplicationUserId = 1, SchoolName = "test-school", ApplicationUser = user };
            dbContext.Users.Add(user);
            dbContext.WebUntisData.Add(untisData);
            await dbContext.SaveChangesAsync();

            var loggerMock = new Mock<ILogger<WebUntisHolidaySyncService>>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            // Setup mock HttpClient to return a fake client. 
            // In real scenarios mocking HttpClient that is used by Refit might require a custom HttpMessageHandler,
            // but we can test the overall interaction.
            var handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://test-server") };
            httpClientFactoryMock.Setup(f => f.CreateClient("WebUntis")).Returns(httpClient);

            var nationalMock = new Mock<INationalHolidayService>();
            nationalMock.Setup(n => n.GetAustrianHolidays(It.IsAny<int>()))
                .Returns(new[] { new Holiday { Name = "National Holiday", StartDate = new DateTime(2026, 10, 26), EndDate = new DateTime(2026, 10, 26) } });

            var schoolMock = new Mock<ISchoolHolidayService>();
            schoolMock.Setup(s => s.FetchAndCacheSchoolHolidaysAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new System.Collections.Generic.List<Holiday> { 
                    new Holiday { Name = "School Holiday", StartDate = new DateTime(2026, 12, 24), EndDate = new DateTime(2026, 12, 31) } 
                });

            // Note: because we can't easily intercept the exact Refit requests without configuring the HttpMessageHandler to return JSON responses for authenticate and getHolidays,
            // we will simulate the behavior of the service without triggering an exception. But since WebUntisHolidaySyncService uses Refit, 
            // it will actually throw if we don't return valid Refit responses.
            
            // Let's configure the mock HttpMessageHandler to return valid JSON for Refit calls.
            var authResponse = new { result = new { sessionId = "test" } };
            var holidayResponse = new { result = new[] { new { id = 1, name = "HW", longName = "Halloween", startDate = 20261031, endDate = 20261031 } } };
            
            handlerMock
               .Protected()
               .SetupSequence<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = System.Net.HttpStatusCode.OK,
                   Content = new StringContent(JsonSerializer.Serialize(authResponse))
               })
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = System.Net.HttpStatusCode.OK,
                   Content = new StringContent(JsonSerializer.Serialize(holidayResponse))
               })
               .ReturnsAsync(new HttpResponseMessage() // For logout
               {
                   StatusCode = System.Net.HttpStatusCode.OK,
                   Content = new StringContent("{}")
               });

            var service = new WebUntisHolidaySyncService(httpClientFactoryMock.Object, dbContext, loggerMock.Object, nationalMock.Object, schoolMock.Object);

            // Act
            await service.SyncHolidaysAsync("test-server", "test-school", "user", "pass");

            // Assert
            var holidaysInDb = await dbContext.Holidays.ToListAsync();
            
            // We expect National Holiday, School Holiday, and Halloween (from Refit mock)
            Assert.Contains(holidaysInDb, h => h.Name == "National Holiday");
            Assert.Contains(holidaysInDb, h => h.Name == "School Holiday");
            Assert.Contains(holidaysInDb, h => h.Name == "Halloween");
        }

        [Theory]
        [InlineData("Wien", "9")]
        [InlineData("Steiermark", "6")]
        [InlineData("Unknown", "9")] // default
        public async Task SchoolHolidayService_MapsStateCorrectly(string bundesland, string expectedSubdivisionCode)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new ApplicationDbContext(options);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var handlerMock = new Mock<HttpMessageHandler>();
            
            var responseJson = "[{\"id\":\"1\",\"startDate\":\"2026-10-26\",\"endDate\":\"2026-11-02\",\"type\":\"school\",\"name\":[{\"language\":\"DE\",\"text\":\"Herbstferien\"}]}]";
            
            string requestedUrl = string.Empty;

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .Callback<HttpRequestMessage, CancellationToken>((req, _) => {
                   requestedUrl = req.RequestUri?.ToString() ?? "";
               })
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = System.Net.HttpStatusCode.OK,
                   Content = new StringContent(responseJson)
               });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://openholidaysapi.org") };
            httpClientFactoryMock.Setup(f => f.CreateClient("DataGvAt")).Returns(httpClient);

            var service = new SchoolHolidayService(httpClientFactoryMock.Object, dbContext);

            // Act
            var holidays = await service.FetchAndCacheSchoolHolidaysAsync(2026, bundesland);

            // Assert
            Assert.NotEmpty(holidays);
            Assert.Contains($"subdivisionCode=AT-{expectedSubdivisionCode}", requestedUrl);
        }
    }
}