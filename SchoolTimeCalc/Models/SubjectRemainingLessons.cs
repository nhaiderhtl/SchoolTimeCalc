namespace SchoolTimeCalc.Models
{
    public class SubjectRemainingLessons
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int CompletedLessons { get; set; }
        public int RemainingLessons { get; set; }
        public int TotalLessons { get; set; }
        public int CanceledLessons { get; set; }
    }
}
