using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ASP.NetCoreProject.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44358/api/")
        };
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WellPage()
        {
            try
            {
                var sessionRole = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionRole"));
                if (sessionRole != null)
                {
                    var sessionName = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionName"));
                    ViewBag.SesRole = sessionRole;
                    ViewBag.SesName = sessionName;
                    return View();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        public IActionResult LoginEmployee()
        {
            return View();
        }

        public IActionResult LoginSupervisor()
        {
            return View();
        }

        public IActionResult LoginAdmin()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult LoginEmployee(EmployeeVM employee)
        {
            EmployeeVM _employee = null;
            var json = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Employees/Login", byteContent).Result;
            var resultView = result.Content.ReadAsStringAsync().Result;
            _employee = JsonConvert.DeserializeObject<EmployeeVM>(resultView);

            if (_employee != null)
            {
                HttpContext.Session.SetString("SessionRole", JsonConvert.SerializeObject("Employee"));
                HttpContext.Session.SetString("SessionName", JsonConvert.SerializeObject(_employee.Name));
                HttpContext.Session.SetString("SessionId", JsonConvert.SerializeObject(_employee.Id));
                return RedirectToAction("WellPage");
            }
            ViewBag.Message = "Wrong Username or password ";
            return View();
        }

        [HttpPost]
        public IActionResult LoginSupervisor(SupervisorVM supervisor)
        {
            SupervisorVM _supervisor = null;
            var json = JsonConvert.SerializeObject(supervisor);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Supervisors/Login", byteContent).Result;
            var resultView = result.Content.ReadAsStringAsync().Result;
            _supervisor = JsonConvert.DeserializeObject<SupervisorVM>(resultView);

            if (_supervisor != null)
            {
                HttpContext.Session.SetString("SessionRole", JsonConvert.SerializeObject("Supervisor"));
                HttpContext.Session.SetString("SessionName", JsonConvert.SerializeObject(_supervisor.Name));
                HttpContext.Session.SetString("SessionId", JsonConvert.SerializeObject(_supervisor.Id));
                return RedirectToAction("WellPage");
            }
            ViewBag.Message = "Wrong Username or password ";
            return View();
        }

        [HttpPost]
        public IActionResult LoginAdmin(AdminVM admin)
        {
            AdminVM _admin = null;
            var json = JsonConvert.SerializeObject(admin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Admins/Login", byteContent).Result;
            var resultView = result.Content.ReadAsStringAsync().Result;
            _admin = JsonConvert.DeserializeObject<AdminVM>(resultView);

            if (_admin != null)
            {
                HttpContext.Session.SetString("SessionRole", JsonConvert.SerializeObject("Admin"));
                HttpContext.Session.SetString("SessionName", JsonConvert.SerializeObject("Admin"));
                return RedirectToAction("WellPage");
            }
            ViewBag.Message = "Wrong Username or password ";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
    }
}
