using System;
using System.Collections.Generic;
using System.Linq;
using Nager.Date;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public class NationalHolidayService : INationalHolidayService
    {
        public IEnumerable<Holiday> GetAustrianHolidays(int year)
        {
            // Formerly DateSystem.GetPublicHolidays in Nager.Date v1
            var publicHolidays = HolidaySystem.GetHolidays(year, CountryCode.AT);
            
            return publicHolidays.Select(h => new Holiday
            {
                Name = h.LocalName,
                StartDate = h.Date,
                EndDate = h.Date,
                SchoolId = "National"
            });
        }
    }
}
