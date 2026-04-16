using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public interface ISchoolHolidayService
    {
        Task<List<Holiday>> FetchAndCacheSchoolHolidaysAsync(int year, string bundesland);
    }
}
