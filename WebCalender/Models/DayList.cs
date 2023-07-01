namespace WebCalender.Models
{
    public class DayList
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime EnglishDate { get; set; }
        public string NepaliDateFormatted { get; set; } = null;
    }
}
