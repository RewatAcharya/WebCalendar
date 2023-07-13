using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WebCalender.Models;
using System.Net.Http;

namespace WebCalender.Controllers
{
    public class ApiUserListController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string UserName, string UserPassword)
        {
            List<UserList>? userList = new List<UserList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiUserList/getusers"))
                {
                    string? apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<UserList>>(apiResponse);
                    foreach (var item in userList)
                    {
                        if(item.UserName == UserName && item.UserPassword == UserPassword)
                        {
                            var claims = new List<Claim>
                               {
                                   new Claim(ClaimTypes.Name, item.UserId.ToString()),
                                   new Claim("UserName", item.UserName),
                                   new Claim(ClaimTypes.Role, item.UserRole)
                               };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            // Sign in the user and issue the authentication cookie
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                            return RedirectToAction("Index", "Home");
                        }
                       
                    }
                }
            }
            return PartialView();
        }

        public IActionResult Register()
        {
            return PartialView();
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

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user and delete the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to the desired page
            return RedirectToAction("Index", "ApiUserList");
        }


        public async Task<IActionResult> Profile(int id)
        {
            List<UserList>? users = new List<UserList>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://apitest.lunarit.com.np/api/apiUserList/getusers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<UserList>>(apiResponse);
                }
            }
            var list = users?.Where(x => x.UserId == id).FirstOrDefault();
           
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserList user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("http://apitest.lunarit.com.np/api/apiuserlist/updateUser", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Logout", "ApiUserList");
        }
    }
}
