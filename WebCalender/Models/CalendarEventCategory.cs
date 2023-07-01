namespace WebCalender.Models
{
    public class CalendarEventCategory
    {
        public short EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string EventColor { get; set; } = null!;
        public string RemarkPosition { get; set; } = null!;
    }
}
