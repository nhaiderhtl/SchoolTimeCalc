using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public class OpenHolidayApiResponse
    {
        public string? Id { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Type { get; set; }
        public List<OpenHolidayName>? Name { get; set; }
    }

    public class OpenHolidayName
    {
        public string? Language { get; set; }
        public string? Text { get; set; }
    }

    public class SchoolHolidayService : ISchoolHolidayService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbContext;

        public SchoolHolidayService(IHttpClientFactory httpClientFactory, ApplicationDbContext dbContext)
        {
            _httpClient = httpClientFactory.CreateClient("DataGvAt");
            _dbContext = dbContext;
        }

        public async Task<List<Holiday>> FetchAndCacheSchoolHolidaysAsync(int year, string bundesland)
        {
            // Check cache first
            var cachedHolidays = await _dbContext.Holidays
                .Where(h => h.SchoolId == "School_" + bundesland && h.StartDate.Year == year)
                .ToListAsync();

            if (cachedHolidays.Any())
            {
                return cachedHolidays;
            }

            var fetchedHolidays = new List<Holiday>();
            try
            {
                // Call a generic open API for school holidays
                var url = $"https://openholidaysapi.org/SchoolHolidays?countryIsoCode=AT&languageIsoCode=DE&validFrom={year}-01-01&validTo={year}-12-31&subdivisionCode=AT-{bundesland.Substring(0, Math.Min(1, bundesland.Length))}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiHolidays = System.Text.Json.JsonSerializer.Deserialize<List<OpenHolidayApiResponse>>(jsonResponse, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (apiHolidays != null)
                    {
                        foreach (var h in apiHolidays)
                        {
                            if (h.Name != null && h.Name.Count > 0 && DateTime.TryParse(h.StartDate, out var sDate) && DateTime.TryParse(h.EndDate, out var eDate))
                            {
                                fetchedHolidays.Add(new Holiday
                                {
                                    Name = h.Name[0].Text ?? "Ferien",
                                    StartDate = sDate,
                                    EndDate = eDate,
                                    SchoolId = "School_" + bundesland
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Return empty list or fallback to empty state
                return fetchedHolidays;
            }

            if (fetchedHolidays.Any())
            {
                _dbContext.Holidays.AddRange(fetchedHolidays);
                await _dbContext.SaveChangesAsync();
            }

            return fetchedHolidays;
        }
    }
}
