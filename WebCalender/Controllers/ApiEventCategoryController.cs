using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Implementation;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WebCalender.Models;

namespace WebCalender.Controllers
{
    
    [Authorize(Roles = "Admin")]
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
                using (var response = await httpClient.PostAsync("http://apitest.lunarit.com.np/api/apiEventCategory/addeventcategory", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return Redirect("GetEventCategories");
        }

        
        public async Task<IActionResult> GetEventCategories()
        {
            List<CalendarEventCategory>? eventList = new List<CalendarEventCategory>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
                {
                    string? apiResponse = await response.Content.ReadAsStringAsync();
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
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
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

                using (var response = await httpClient.PutAsync("http://apitest.lunarit.com.np/api/apiEventCategory/updateeventcategory", content))
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
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiEventCategory/geteventcategories"))
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
                using (var response = await httpClient.DeleteAsync("http://apitest.lunarit.com.np/api/apiEventCategory/deleteeventcategory/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    
                }
            }
            return RedirectToAction("GetEventCategories");
        }
    }
}
