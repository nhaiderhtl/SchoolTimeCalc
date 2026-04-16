using System.Text.Json;
using SchoolTimeCalc.Services;
using Xunit;

namespace SchoolTimeCalc.Tests
{
    public class HolidayDtoTests
    {
        [Fact]
        public void CanDeserialize_UntisHolidayDto()
        {
            // Arrange
            var json = @"
            {
                ""id"": 123,
                ""name"": ""HW"",
                ""longName"": ""Halloween"",
                ""startDate"": 20261031,
                ""endDate"": 20261101
            }";

            // Act
            var dto = JsonSerializer.Deserialize<UntisHolidayDto>(json);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(123, dto.Id);
            Assert.Equal("HW", dto.Name);
            Assert.Equal("Halloween", dto.LongName);
            Assert.Equal(20261031, dto.StartDate);
            Assert.Equal(20261101, dto.EndDate);
        }

        [Fact]
        public void CanSerialize_UntisHolidayDto()
        {
            // Arrange
            var dto = new UntisHolidayDto
            {
                Id = 456,
                Name = "XM",
                LongName = "Christmas",
                StartDate = 20261224,
                EndDate = 20261226
            };

            // Act
            var json = JsonSerializer.Serialize(dto);

            // Assert
            Assert.Contains("\"id\":456", json);
            Assert.Contains("\"name\":\"XM\"", json);
            Assert.Contains("\"longName\":\"Christmas\"", json);
            Assert.Contains("\"startDate\":20261224", json);
            Assert.Contains("\"endDate\":20261226", json);
        }
    }
}
