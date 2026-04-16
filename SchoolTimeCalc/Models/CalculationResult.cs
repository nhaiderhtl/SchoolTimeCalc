using System;
using System.Collections.Generic;

namespace SchoolTimeCalc.Models
{
    public class CalculationResult
    {
        public int TotalRemainingDays { get; set; }
        public int TotalRemainingLessons { get; set; }
        public DateTime EndDate { get; set; }
        public List<SubjectRemainingLessons> SubjectLessons { get; set; } = new();
    }
}
