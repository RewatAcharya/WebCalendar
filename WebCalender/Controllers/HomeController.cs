﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
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
            if (month == null)
            {
                DateTime today = DateTime.Today;
                var nepaliMonth = today.AddYears(56).AddMonths(8).AddDays(15);
                month = nepaliMonth.Month;
            }
            
            List<CalendarEventDate> eventDayList = new List<CalendarEventDate>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://apitest.lunarit.com.np/api/apiEventDate/GetEventDayList/2080/{month}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventDayList = JsonConvert.DeserializeObject<List<CalendarEventDate>>(apiResponse);
                }
            }


            List<CalendarEventCategory> eventList = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }



            List<DayList> dayList = new List<DayList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://apitest.lunarit.com.np/api/apidaylist/getdaylist/2080/{month}"))
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