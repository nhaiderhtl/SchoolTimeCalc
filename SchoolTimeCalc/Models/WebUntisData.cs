namespace SchoolTimeCalc.Models
{
    public class WebUntisData
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public string SchoolName { get; set; } = string.Empty;
        public string? SubjectsJson { get; set; }
        public string? TeachersJson { get; set; }
        public string? RoomsJson { get; set; }
        public string? LessonsJson { get; set; }
        public DateTime? LastHolidaySync { get; set; }
    }
}
