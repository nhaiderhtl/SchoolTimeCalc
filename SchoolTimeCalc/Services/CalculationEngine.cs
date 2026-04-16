using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public class CalculationEngine
    {
        private readonly ApplicationDbContext _dbContext;

        public CalculationEngine(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CalculateRemainingSchoolDaysAsync(int userId)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return 0;
            }

            var webUntisData = await _dbContext.WebUntisData
                .FirstOrDefaultAsync(d => d.ApplicationUserId == userId);

            if (webUntisData == null || string.IsNullOrEmpty(webUntisData.SchoolName))
            {
                return 0; 
            }

            string schoolId = webUntisData.SchoolName;

            var holidays = await _dbContext.Holidays
                .Where(h => h.SchoolId == schoolId)
                .ToListAsync();

            DateTime current = DateTime.Today;
            // Assuming end of school year is roughly early July in Austria (e.g., July 4th)
            DateTime targetEndDate = new DateTime(current.Year + (current.Month >= 7 ? 1 : 0), 7, 4);

            int remainingDays = 0;

            while (current <= targetEndDate)
            {
                // Skip weekends
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                {
                    // Check if current day is in any holiday
                    bool isHoliday = holidays.Any(h => current >= h.StartDate && current <= h.EndDate);
                    
                    if (!isHoliday)
                    {
                        remainingDays++;
                    }
                }
                current = current.AddDays(1);
            }

            return remainingDays;
        }
    }
}