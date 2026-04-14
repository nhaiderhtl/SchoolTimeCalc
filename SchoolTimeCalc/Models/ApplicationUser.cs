namespace SchoolTimeCalc.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Bundesland { get; set; }
    }
}
