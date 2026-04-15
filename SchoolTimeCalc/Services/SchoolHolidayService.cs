using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public class SchoolHolidayService
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
                .Where(h => h.Type == "School" && h.Date.Year == year && h.Bundesland == bundesland)
                .ToListAsync();

            if (cachedHolidays.Any())
            {
                return cachedHolidays;
            }

            var fetchedHolidays = new List<Holiday>();
            try
            {
                // Mock API call to data.gv.at
                // e.g. var response = await _httpClient.GetFromJsonAsync<HolidayApiResponse>($"...{year}...");
                // Note: The specific URL/model needs to be aligned with the actual data.gv.at structure later.
                
                fetchedHolidays.Add(new Holiday
                {
                    Name = "Weihnachtsferien",
                    Date = new DateTime(year, 12, 25),
                    Type = "School",
                    Bundesland = bundesland
                });
                // Soft fail gracefully if there's an actual exception with actual API
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
