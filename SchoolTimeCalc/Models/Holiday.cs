using System;

namespace SchoolTimeCalc.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty; // e.g. "National", "School"
        public string? Bundesland { get; set; } // optional, e.g. "W", "NÖ", etc.
    }
}
