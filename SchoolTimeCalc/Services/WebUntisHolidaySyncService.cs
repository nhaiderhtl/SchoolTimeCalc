using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Refit;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public class WebUntisHolidaySyncService : IHolidaySyncService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<WebUntisHolidaySyncService> _logger;

        public WebUntisHolidaySyncService(IHttpClientFactory httpClientFactory, ApplicationDbContext dbContext, ILogger<WebUntisHolidaySyncService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task SyncHolidaysAsync(string server, string schoolName, string username, string password, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting holiday sync for school {SchoolName} on server {Server}", schoolName, server);

            var httpClient = _httpClientFactory.CreateClient("WebUntis");
            httpClient.BaseAddress = new Uri($"https://{server}");
            var client = RestService.For<IWebUntisClient>(httpClient);

            var authReq = new UntisRpcRequest
            {
                Method = "authenticate",
                Params = new UntisAuthRequest
                {
                    User = username,
                    Password = password
                }
            };

            var authRes = await client.AuthenticateAsync(authReq, schoolName);
            if (authRes?.Error != null || authRes?.Result == null)
            {
                _logger.LogError("Authentication failed: {Error}", authRes?.Error?.Message);
                return;
            }

            var cookie = $"JSESSIONID={authRes.Result.SessionId}";

            try
            {
                var holidayReq = new UntisRpcRequest
                {
                    Method = "getHolidays"
                };

                var holidayRes = await client.GetHolidaysAsync(holidayReq, schoolName, cookie);
                if (holidayRes?.Error != null)
                {
                    _logger.LogError("Failed to get holidays: {Error}", holidayRes.Error.Message);
                    return;
                }

                var dtos = new List<UntisHolidayDto>();
                if (holidayRes?.Result.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in holidayRes.Result.EnumerateArray())
                    {
                        var dto = JsonSerializer.Deserialize<UntisHolidayDto>(item.GetRawText());
                        if (dto != null)
                        {
                            dtos.Add(dto);
                        }
                    }
                }

                var existingHolidays = await _dbContext.Holidays.Where(h => h.SchoolId == schoolName).ToListAsync(cancellationToken);
                var existingDict = existingHolidays.ToDictionary(h => h.Name + h.StartDate.ToString("yyyyMMdd") + h.EndDate.ToString("yyyyMMdd"));

                foreach (var dto in dtos)
                {
                    if (DateTime.TryParseExact(dto.StartDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDate) &&
                        DateTime.TryParseExact(dto.EndDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate))
                    {
                        var key = dto.LongName + dto.StartDate.ToString() + dto.EndDate.ToString();
                        if (!existingDict.ContainsKey(key))
                        {
                            _dbContext.Holidays.Add(new Holiday
                            {
                                Name = dto.LongName,
                                StartDate = startDate,
                                EndDate = endDate,
                                SchoolId = schoolName
                            });
                        }
                    }
                }

                var webUntisData = await _dbContext.Set<WebUntisData>().FirstOrDefaultAsync(w => w.SchoolName == schoolName, cancellationToken);
                if (webUntisData != null)
                {
                    webUntisData.LastHolidaySync = DateTime.UtcNow;
                    _dbContext.Update(webUntisData);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Holiday sync completed successfully for {SchoolName}", schoolName);
            }
            finally
            {
                try 
                {
                    var logoutReq = new UntisRpcRequest { Method = "logout" };
                    await client.LogoutAsync(logoutReq, schoolName, cookie);
                }
                catch { /* Ignore logout errors */ }
            }
        }
    }
}
