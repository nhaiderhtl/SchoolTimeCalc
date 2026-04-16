using System;
using System.Threading.Tasks;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public interface ICalculationService
    {
        Task<CalculationResult> CalculateRemainingTimeAsync(string username);
        Task<WeekViewData> GetWeekTimetableAsync(string username, DateTime dateInWeek);
    }
}
