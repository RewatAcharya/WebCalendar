﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Implementation;
using Newtonsoft.Json;
using System.Text;
using WebCalender.Models;

namespace WebCalender.Controllers
{
    public class ApiEventCategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult AddEventCategories() => View();

        [HttpPost]
        public async Task<IActionResult> AddEventCategories(CalendarEventCategory calenderEventCategory)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(calenderEventCategory), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://apitest.lunarit.com.np/api/apiEventCategory/addeventcategory", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return Redirect("GetEventCategories");
        }

        public async Task<IActionResult> GetEventCategories()
        {
            List<CalendarEventCategory> eventList = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }
            return View(eventList);
        }

        
        public async Task<IActionResult> UpdateEventCategory(int id)
        {
            List<CalendarEventCategory> eventList = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }
            var list = eventList?.Where(x => x.EventId == id).FirstOrDefault();
            return View(list);
        }

        

        [HttpPost]
        public async Task<IActionResult> UpdateEventCategory(CalendarEventCategory calendarEventCategory)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(calendarEventCategory), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://apitest.lunarit.com.np/api/apiEventCategory/updateeventcategory", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("GetEventCategories");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEventCategory(int id)
        {
            List<CalendarEventCategory> eventList = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<CalendarEventCategory>>(apiResponse);
                }
            }
            var list = eventList?.Where(x => x.EventId == id).FirstOrDefault();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://apitest.lunarit.com.np/api/apiEventCategory/deleteeventcategory/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    
                }
            }
            return RedirectToAction("GetEventCategories");
        }
    }
}