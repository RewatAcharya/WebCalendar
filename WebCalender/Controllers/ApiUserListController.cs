using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebCalender.Models;

namespace WebCalender.Controllers
{
    public class ApiUserListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string UserName, string UserPassword)
        {
            List<UserList>? eventList = new List<UserList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiUserList/getusers"))
                {
                    string? apiResponse = await response.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<UserList>>(apiResponse);
                    foreach (var item in eventList)
                    {
                        if(item.UserName == UserName && item.UserPassword == UserPassword)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserList userList)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(userList), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://apitest.lunarit.com.np/api/apiuserlist/adduser", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index", "ApiUserList");            
        }


    }
}
