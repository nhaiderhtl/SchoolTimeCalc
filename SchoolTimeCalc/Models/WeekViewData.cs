using System;
using System.Collections.Generic;

namespace SchoolTimeCalc.Models
{
    public class WeekViewData
    {
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public List<WeekLesson> Lessons { get; set; } = new();
        public List<WeekHoliday> Holidays { get; set; } = new();
    }

    public class WeekLesson
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsCanceled => Status == "CANCEL" || Status == "cancelled" || Status == "CANCELLED";
    }

    public class WeekHoliday
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
