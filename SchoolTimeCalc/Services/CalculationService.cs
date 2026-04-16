using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolTimeCalc.Data;
using SchoolTimeCalc.Models;

namespace SchoolTimeCalc.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly ApplicationDbContext _dbContext;

        public CalculationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CalculationResult> CalculateRemainingTimeAsync(string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return new CalculationResult();

            var webUntisData = await _dbContext.WebUntisData.FirstOrDefaultAsync(d => d.ApplicationUserId == user.Id);
            if (webUntisData == null || string.IsNullOrEmpty(webUntisData.LessonsJson))
                return new CalculationResult();

            string schoolId = webUntisData.SchoolName;

            var holidays = await _dbContext.Holidays
                .Where(h => h.SchoolId == schoolId)
                .ToListAsync();

            List<UntisLesson> lessons;
            try
            {
                lessons = JsonSerializer.Deserialize<List<UntisLesson>>(webUntisData.LessonsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UntisLesson>();
            }
            catch
            {
                lessons = new List<UntisLesson>();
            }

            var subjectMap = new Dictionary<int, string>();
            if (!string.IsNullOrEmpty(webUntisData.SubjectsJson))
            {
                try
                {
                    var subjects = JsonSerializer.Deserialize<List<UntisSubject>>(webUntisData.SubjectsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (subjects != null)
                    {
                        foreach (var sub in subjects)
                        {
                            subjectMap[sub.Id] = sub.Name;
                        }
                    }
                }
                catch { }
            }

            var today = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
            var futureLessons = lessons
                .Where(l => l.Date >= today)
                .Where(l => l.Stat != "CANCEL" && l.Stat != "cancelled" && l.Stat != "CANCELLED")
                .ToList();

            var activeDays = new HashSet<int>();
            var subjectCounts = new Dictionary<int, int>();
            var subjectTotals = new Dictionary<int, int>();
            var subjectCanceled = new Dictionary<int, int>();
            int totalRemainingLessons = 0;
            int lastLessonDateInt = today;

            foreach (var lesson in lessons)
            {
                bool isCanceled = lesson.Stat == "CANCEL" || lesson.Stat == "cancelled" || lesson.Stat == "CANCELLED";
                foreach (var su in lesson.Su ?? Enumerable.Empty<UntisLessonSubject>())
                {
                    if (!subjectTotals.ContainsKey(su.Id)) subjectTotals[su.Id] = 0;
                    subjectTotals[su.Id]++;
                    
                    if (isCanceled)
                    {
                        if (!subjectCanceled.ContainsKey(su.Id)) subjectCanceled[su.Id] = 0;
                        subjectCanceled[su.Id]++;
                    }
                }
            }

            foreach (var lesson in futureLessons)
            {
                int y = lesson.Date / 10000;
                int m = (lesson.Date / 100) % 100;
                int d = lesson.Date % 100;
                var lessonDate = new DateTime(y, m, d);

                if (lessonDate.DayOfWeek == DayOfWeek.Saturday || lessonDate.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                bool isHoliday = holidays.Any(h => lessonDate >= h.StartDate && lessonDate <= h.EndDate);
                if (isHoliday)
                    continue;

                activeDays.Add(lesson.Date);
                totalRemainingLessons++;
                if (lesson.Date > lastLessonDateInt)
                    lastLessonDateInt = lesson.Date;

                foreach (var su in lesson.Su ?? Enumerable.Empty<UntisLessonSubject>())
                {
                    if (subjectCounts.ContainsKey(su.Id))
                        subjectCounts[su.Id]++;
                    else
                        subjectCounts[su.Id] = 1;
                }
            }

            var result = new CalculationResult
            {
                TotalRemainingDays = activeDays.Count,
                TotalRemainingLessons = totalRemainingLessons,
                EndDate = new DateTime(lastLessonDateInt / 10000, (lastLessonDateInt / 100) % 100, lastLessonDateInt % 100)
            };

            foreach (var kvp in subjectCounts)
            {
                result.SubjectLessons.Add(new SubjectRemainingLessons
                {
                    SubjectId = kvp.Key,
                    SubjectName = subjectMap.ContainsKey(kvp.Key) ? subjectMap[kvp.Key] : $"Subject {kvp.Key}",
                    RemainingLessons = kvp.Value,
                    TotalLessons = subjectTotals.ContainsKey(kvp.Key) ? subjectTotals[kvp.Key] : kvp.Value,
                    CanceledLessons = subjectCanceled.ContainsKey(kvp.Key) ? subjectCanceled[kvp.Key] : 0
                });
            }

            return result;
        }
        
        public async Task<WeekViewData> GetWeekTimetableAsync(string username, DateTime dateInWeek)
        {
            var result = new WeekViewData();

            int diff = (7 + (dateInWeek.DayOfWeek - DayOfWeek.Monday)) % 7;
            result.WeekStartDate = dateInWeek.AddDays(-1 * diff).Date;
            result.WeekEndDate = result.WeekStartDate.AddDays(4).Date; // Monday to Friday

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return result;

            var webUntisData = await _dbContext.WebUntisData.FirstOrDefaultAsync(d => d.ApplicationUserId == user.Id);
            if (webUntisData == null)
                return result;

            string schoolId = webUntisData.SchoolName;

            var holidays = await _dbContext.Holidays
                .Where(h => h.SchoolId == schoolId && h.EndDate >= result.WeekStartDate && h.StartDate <= result.WeekEndDate)
                .ToListAsync();

            foreach (var h in holidays)
            {
                result.Holidays.Add(new WeekHoliday
                {
                    Id = h.Id,
                    Name = h.Name,
                    StartDate = h.StartDate,
                    EndDate = h.EndDate
                });
            }

            if (string.IsNullOrEmpty(webUntisData.LessonsJson))
                return result;

            List<UntisLesson> lessons;
            try
            {
                lessons = JsonSerializer.Deserialize<List<UntisLesson>>(webUntisData.LessonsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UntisLesson>();
            }
            catch
            {
                lessons = new List<UntisLesson>();
            }

            var subjectMap = new Dictionary<int, string>();
            if (!string.IsNullOrEmpty(webUntisData.SubjectsJson))
            {
                try
                {
                    var subjects = JsonSerializer.Deserialize<List<UntisSubject>>(webUntisData.SubjectsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (subjects != null)
                    {
                        foreach (var sub in subjects)
                        {
                            subjectMap[sub.Id] = sub.Name;
                        }
                    }
                }
                catch { }
            }

            int startInt = int.Parse(result.WeekStartDate.ToString("yyyyMMdd"));
            int endInt = int.Parse(result.WeekEndDate.ToString("yyyyMMdd"));

            var weekLessons = lessons.Where(l => l.Date >= startInt && l.Date <= endInt).ToList();

            foreach (var l in weekLessons)
            {
                var lessonDate = new DateTime(l.Date / 10000, (l.Date / 100) % 100, l.Date % 100);
                
                string subjName = "Unknown";
                if (l.Su != null && l.Su.Any())
                {
                    int firstSubjId = l.Su.First().Id;
                    if (subjectMap.ContainsKey(firstSubjId))
                        subjName = subjectMap[firstSubjId];
                }

                result.Lessons.Add(new WeekLesson
                {
                    Id = l.Id,
                    Date = lessonDate,
                    StartTime = new TimeSpan(l.StartTime / 100, l.StartTime % 100, 0),
                    EndTime = new TimeSpan(l.EndTime / 100, l.EndTime % 100, 0),
                    SubjectName = subjName,
                    Status = l.Stat ?? ""
                });
            }

            return result;
        }

        private class UntisLesson
        {
            public int Id { get; set; }
            public int Date { get; set; }
            public int StartTime { get; set; }
            public int EndTime { get; set; }
            public List<UntisLessonSubject>? Su { get; set; }
            public string? Stat { get; set; }
        }
        
        private class UntisLessonSubject
        {
            public int Id { get; set; }
        }

        private class UntisSubject
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string LongName { get; set; } = string.Empty;
        }
    }
}
