namespace SchoolTimeCalc.Models
{
    public class SubjectRemainingLessons
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int RemainingLessons { get; set; }
    }
}
