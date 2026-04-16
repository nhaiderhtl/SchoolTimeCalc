using System.Collections.Generic;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public interface INationalHolidayService
    {
        IEnumerable<Holiday> GetAustrianHolidays(int year);
    }
}
