namespace WebCalender.Models
{
    public class CalendarEventDate
    {
        public long DateId { get; set; }
        public string NepaliDate { get; set; } = null!;
        public string EventName { get; set; } = null!;
        public string EventDescription { get; set; } = null!;
    }
}
