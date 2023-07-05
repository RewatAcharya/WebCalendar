using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.JSInterop.Implementation;
using Newtonsoft.Json;
using System.Text;
using WebCalender.Models;

namespace WebCalender.Controllers
{
    public class ApiEventDateController : Controller
    {
        public async Task<IActionResult> Index(string param)
        {
            List<CalendarEventCategory>? events = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    events = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }
            var elist=new SelectList(events, nameof(CalendarEventCategory.EventName), nameof(CalendarEventCategory.EventName));          
            ViewBag.eventList=elist;

            CalendarEventDate date = new CalendarEventDate
            {
                NepaliDate = param
            };
            return PartialView(date);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string param)
        {
            string dateString = param;
            DateTime date = DateTime.ParseExact(dateString, "yyyy-M-d", null);
            int month = date.Month;

            List<CalendarEventCategory>? eventCategory = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventCategory = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }
            var elist = new SelectList(eventCategory, nameof(CalendarEventCategory.EventName), nameof(CalendarEventCategory.EventName));
            ViewBag.eventList = elist;


            List<CalendarEventDate> events = new List<CalendarEventDate>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventDate/GetEventDayList/2080/" + month))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    events = JsonConvert.DeserializeObject<List<CalendarEventDate>>(apiResponse);
                }
            }
            var list = events?.Where(x => x.NepaliDate == param).FirstOrDefault();
            return PartialView(list);



          /*  List<CalendarEventDate>? events = new List<CalendarEventDate>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventDate/GetEventDayList/2080/" + month))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    events = JsonConvert.DeserializeObject<List<CalendarEventDate>>(apiResponse);
                }
            }
            var elist = new SelectList(events, nameof(CalendarEventCategory.EventName), nameof(CalendarEventCategory.EventName));
            ViewBag.eventList = elist;
            foreach (var item in events)
            {
                if(item.NepaliDate == param)
                {
                    CalendarEventDate dates = new CalendarEventDate
                    {
                        
                        EventDescription = item.EventDescription,
                        NepaliDate = param
                    };
                    return PartialView(dates);
                }
            }
            return RedirectToAction("Index");*/
        }

        [HttpPost]
        public async Task<IActionResult> Index(CalendarEventDate calendarEventDate)
        {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(calendarEventDate), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("http://apitest.lunarit.com.np/api/apiEventDate/AddCalendarEventDate", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
             return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> Read()
        {
            return View();
        }
        
        public async Task<IActionResult> ReadList(int? month)
        {
            if (month == null)
            {
                DateTime dateTime = DateTime.Today;
                DateTime nepaliYear = dateTime.AddYears(56).AddMonths(8).AddDays(15);
                month = nepaliYear.Month;
            }
            List<CalendarEventDate>? eventList = new List<CalendarEventDate>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventDate/GetEventDayList/2080/" + month))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<CalendarEventDate>>(apiResponse);
                }
            }
            return PartialView("_ReadList", eventList);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("http://apitest.lunarit.com.np/api/apiEventDate/DeleteCalendarEventDate/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Read");
        }
    }
}
