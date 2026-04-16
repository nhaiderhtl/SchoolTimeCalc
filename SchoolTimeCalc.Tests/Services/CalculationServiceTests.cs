using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;
using SchoolTimeCalc.Services;
using Xunit;

namespace SchoolTimeCalc.Tests.Services
{
    public class CalculationServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private async Task SetupUserAndDataAsync(ApplicationDbContext dbContext, string username, string schoolId, string lessonsJson, string subjectsJson = "")
        {
            var user = new ApplicationUser { Id = 1, Username = username };
            var untisData = new WebUntisData
            {
                Id = 1,
                ApplicationUserId = 1,
                SchoolName = schoolId,
                LessonsJson = lessonsJson,
                SubjectsJson = subjectsJson
            };

            dbContext.Users.Add(user);
            dbContext.WebUntisData.Add(untisData);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task CalculateRemainingTimeAsync_EmptyTimetable_ReturnsZero()
        {
            // Arrange
            using var dbContext = GetDbContext();
            await SetupUserAndDataAsync(dbContext, "testuser", "School1", "[]");
            var service = new CalculationService(dbContext);

            // Act
            var result = await service.CalculateRemainingTimeAsync("testuser");

            // Assert
            Assert.Equal(0, result.TotalRemainingDays);
            Assert.Equal(0, result.TotalRemainingLessons);
            Assert.Empty(result.SubjectLessons);
        }

        [Fact]
        public async Task CalculateRemainingTimeAsync_SimpleOneDayTimetable_ReturnsCorrectValues()
        {
            // Arrange
            using var dbContext = GetDbContext();
            
            // Generate a date string for next Tuesday
            var nextTuesday = DateTime.Today;
            while (nextTuesday.DayOfWeek != DayOfWeek.Tuesday)
                nextTuesday = nextTuesday.AddDays(1);
                
            int dateInt = nextTuesday.Year * 10000 + nextTuesday.Month * 100 + nextTuesday.Day;

            var lessons = new[]
            {
                new { Id = 1, Date = dateInt, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" },
                new { Id = 2, Date = dateInt, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" },
                new { Id = 3, Date = dateInt, Su = new[] { new { Id = 102 } }, Stat = "REGULAR" }
            };

            await SetupUserAndDataAsync(dbContext, "testuser", "School1", JsonSerializer.Serialize(lessons));
            var service = new CalculationService(dbContext);

            // Act
            var result = await service.CalculateRemainingTimeAsync("testuser");

            // Assert
            Assert.Equal(1, result.TotalRemainingDays);
            Assert.Equal(3, result.TotalRemainingLessons);
            Assert.Equal(2, result.SubjectLessons.Count);
        }

        [Fact]
        public async Task CalculateRemainingTimeAsync_ExcludesWeekends()
        {
            // Arrange
            using var dbContext = GetDbContext();
            
            var nextSaturday = DateTime.Today;
            while (nextSaturday.DayOfWeek != DayOfWeek.Saturday)
                nextSaturday = nextSaturday.AddDays(1);
                
            var nextSunday = nextSaturday.AddDays(1);
            var nextMonday = nextSaturday.AddDays(2);
            
            int satInt = nextSaturday.Year * 10000 + nextSaturday.Month * 100 + nextSaturday.Day;
            int sunInt = nextSunday.Year * 10000 + nextSunday.Month * 100 + nextSunday.Day;
            int monInt = nextMonday.Year * 10000 + nextMonday.Month * 100 + nextMonday.Day;

            var lessons = new[]
            {
                new { Id = 1, Date = satInt, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" },
                new { Id = 2, Date = sunInt, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" },
                new { Id = 3, Date = monInt, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" }
            };

            await SetupUserAndDataAsync(dbContext, "testuser", "School1", JsonSerializer.Serialize(lessons));
            var service = new CalculationService(dbContext);

            // Act
            var result = await service.CalculateRemainingTimeAsync("testuser");

            // Assert
            Assert.Equal(1, result.TotalRemainingDays); // Only Monday
            Assert.Equal(1, result.TotalRemainingLessons); // Only Monday's lesson
        }

        [Fact]
        public async Task CalculateRemainingTimeAsync_ExcludesHolidays()
        {
            // Arrange
            using var dbContext = GetDbContext();
            
            var holidayStart = DateTime.Today.AddDays(5);
            var holidayEnd = DateTime.Today.AddDays(10);
            
            dbContext.Holidays.Add(new Holiday 
            { 
                Id = 1, 
                SchoolId = "School1", 
                Name = "Spring Break", 
                StartDate = holidayStart, 
                EndDate = holidayEnd 
            });

            // Find a valid weekday within the holiday
            var holidayDay = holidayStart;
            while (holidayDay.DayOfWeek == DayOfWeek.Saturday || holidayDay.DayOfWeek == DayOfWeek.Sunday)
                holidayDay = holidayDay.AddDays(1);

            // Find a valid weekday after the holiday
            var regularDay = holidayEnd.AddDays(1);
            while (regularDay.DayOfWeek == DayOfWeek.Saturday || regularDay.DayOfWeek == DayOfWeek.Sunday)
                regularDay = regularDay.AddDays(1);

            int holInt = holidayDay.Year * 10000 + holidayDay.Month * 100 + holidayDay.Day;
            int regInt = regularDay.Year * 10000 + regularDay.Month * 100 + regularDay.Day;

            var lessons = new[]
            {
                new { Id = 1, Date = holInt, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" },
                new { Id = 2, Date = regInt, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" }
            };

            await SetupUserAndDataAsync(dbContext, "testuser", "School1", JsonSerializer.Serialize(lessons));
            var service = new CalculationService(dbContext);

            // Act
            var result = await service.CalculateRemainingTimeAsync("testuser");

            // Assert
            Assert.Equal(1, result.TotalRemainingDays);
            Assert.Equal(1, result.TotalRemainingLessons);
        }

        [Fact]
        public async Task CalculateRemainingTimeAsync_ExcludesCancelledLessons()
        {
            // Arrange
            using var dbContext = GetDbContext();
            
            var nextTuesday = DateTime.Today;
            while (nextTuesday.DayOfWeek != DayOfWeek.Tuesday)
                nextTuesday = nextTuesday.AddDays(1);
                
            int dateInt = nextTuesday.Year * 10000 + nextTuesday.Month * 100 + nextTuesday.Day;

            var lessons = new[]
            {
                new { Id = 1, Date = dateInt, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" },
                new { Id = 2, Date = dateInt, Su = new[] { new { Id = 102 } }, Stat = "CANCEL" },
                new { Id = 3, Date = dateInt, Su = new[] { new { Id = 103 } }, Stat = "cancelled" }
            };

            await SetupUserAndDataAsync(dbContext, "testuser", "School1", JsonSerializer.Serialize(lessons));
            var service = new CalculationService(dbContext);

            // Act
            var result = await service.CalculateRemainingTimeAsync("testuser");

            // Assert
            Assert.Equal(1, result.TotalRemainingDays);
            Assert.Equal(1, result.TotalRemainingLessons); // Only the REGULAR lesson
        }
        
        [Fact]
        public async Task CalculateRemainingTimeAsync_DynamicEndDate_ReturnsCorrectDate()
        {
            // Arrange
            using var dbContext = GetDbContext();
            
            var nextTuesday = DateTime.Today;
            while (nextTuesday.DayOfWeek != DayOfWeek.Tuesday)
                nextTuesday = nextTuesday.AddDays(1);
                
            var nextWeekTuesday = nextTuesday.AddDays(7);
                
            int dateInt1 = nextTuesday.Year * 10000 + nextTuesday.Month * 100 + nextTuesday.Day;
            int dateInt2 = nextWeekTuesday.Year * 10000 + nextWeekTuesday.Month * 100 + nextWeekTuesday.Day;

            var lessons = new[]
            {
                new { Id = 1, Date = dateInt1, Su = new[] { new { Id = 101 } }, Stat = "REGULAR" },
                new { Id = 2, Date = dateInt2, Su = new[] { new { Id = 102 } }, Stat = "REGULAR" }
            };

            await SetupUserAndDataAsync(dbContext, "testuser", "School1", JsonSerializer.Serialize(lessons));
            var service = new CalculationService(dbContext);

            // Act
            var result = await service.CalculateRemainingTimeAsync("testuser");

            // Assert
            Assert.Equal(nextWeekTuesday.Date, result.EndDate.Date);
        }
    }
}
