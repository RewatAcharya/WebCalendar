using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using WebCalender.Models;

namespace WebCalender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public async Task<IActionResult> Index(int? month)
        {
            
            //getting month dynamically....
            if (month == null)
            {
                DateTime dateTime = DateTime.Today;
                DateTime nepaliYear = dateTime.AddYears(56).AddMonths(8).AddDays(15);
                month = nepaliYear.Month;
            }

                string monthName = "";
                switch (month)
                {
                    case 1:
                        monthName = "Baisakh";
                        break;
                    case 2:
                        monthName = "Jestha";
                        break;
                    case 3:
                        monthName = "Asadh";
                        break;
                    case 4:
                        monthName = "Sharawan";
                        break;
                    case 5:
                        monthName = "Bhadra";
                        break;
                    case 6:
                        monthName = "Asoj";
                        break;
                    case 7:
                        monthName = "Kartik";
                        break;
                    case 8:
                        monthName = "Mangisr";
                        break;
                    case 9:
                        monthName = "Poush";
                        break;
                    case 10:
                        monthName = "Magh";
                        break;
                    case 11:
                        monthName = "Falgun";
                        break;
                    case 12:
                        monthName = "Chaitra";
                        break;
                    default:
                        break;
                }
               ViewBag.MonthName = monthName;
            





            var selectedEmonth = month;
            string selectedEmonthName = string.Empty;
            string selectedEmonthName2 = string.Empty;

            if (selectedEmonth != null)
            {
                DateTimeFormatInfo dateFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
                int adjustedMonth = (int)selectedEmonth + 3;

                if (adjustedMonth <= 11)
                {
                    selectedEmonthName = dateFormatInfo.GetMonthName(adjustedMonth);
                    selectedEmonthName2 = dateFormatInfo.GetMonthName(adjustedMonth + 1);
                }
                else if (adjustedMonth == 12)
                {
                    selectedEmonthName = dateFormatInfo.GetMonthName(adjustedMonth);
                    selectedEmonthName2 = dateFormatInfo.GetMonthName(adjustedMonth - 11);
                }
                else
                {
                    selectedEmonthName = dateFormatInfo.GetMonthName(adjustedMonth - 12);
                    selectedEmonthName2 = dateFormatInfo.GetMonthName(adjustedMonth - 11);
                }
            }
            else
            {
                selectedEmonthName = "Unknown";
            }
            //shows english month in Jun/Jul format....
            ViewBag.EnglishMonth = selectedEmonthName + "/" + selectedEmonthName2;


            List<CalendarEventDate>? eventDayList = new List<CalendarEventDate>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"http://apitest.lunarit.com.np/api/apiEventDate/GetEventDayList/2080/{month}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventDayList = JsonConvert.DeserializeObject<List<CalendarEventDate>>(apiResponse);
                }
            }


            List<CalendarEventCategory>? eventList = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }



            List<DayList>? dayList = new List<DayList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"http://apitest.lunarit.com.np/api/apidaylist/getdaylist/2080/{month}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dayList = JsonConvert.DeserializeObject<List<DayList>>(apiResponse);
                }
            }
            ViewBag.EventDayList = eventDayList;
            ViewBag.EventList = eventList;
            return View(dayList);
        }

    }
}