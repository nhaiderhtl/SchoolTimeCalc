using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolTimeCalc.Data;

namespace SchoolTimeCalc.Services
{
    public class HolidaySyncBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HolidaySyncBackgroundService> _logger;

        public HolidaySyncBackgroundService(IServiceProvider serviceProvider, ILogger<HolidaySyncBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("HolidaySyncBackgroundService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SyncAllUsersHolidaysAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing holiday sync.");
                }

                // Run every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            _logger.LogInformation("HolidaySyncBackgroundService is stopping.");
        }

        private async Task SyncAllUsersHolidaysAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var syncService = scope.ServiceProvider.GetRequiredService<IHolidaySyncService>();

            // Get all active webuntis data
            var webUntisData = await dbContext.WebUntisData
                .Include(w => w.ApplicationUser)
                .ToListAsync(stoppingToken);

            foreach (var data in webUntisData)
            {
                if (string.IsNullOrEmpty(data.SchoolName) || string.IsNullOrEmpty(data.Server) || string.IsNullOrEmpty(data.EncryptedPassword) || string.IsNullOrEmpty(data.Username))
                {
                    _logger.LogWarning("Missing credentials for WebUntisData ID {Id}. Skipping.", data.Id);
                    continue;
                }

                try
                {
                    await syncService.SyncHolidaysAsync(data.Server, data.SchoolName, data.Username, data.EncryptedPassword, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to sync holidays for school {SchoolName}", data.SchoolName);
                }
            }
        }
    }
}
