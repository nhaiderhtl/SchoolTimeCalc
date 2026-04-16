using System;

namespace SchoolTimeCalc.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SchoolId { get; set; } = string.Empty;
    }
}
