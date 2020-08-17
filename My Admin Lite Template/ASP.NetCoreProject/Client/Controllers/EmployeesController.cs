using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASP.NetCoreProject.Models;
using ASP.NetCoreProject.ViewModels;
using Client.Pdf;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class EmployeesController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44358/api/")
        };
        public IActionResult Index()
        {
            try
            {
                var sessionRole = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionRole"));
                if (sessionRole.ToString() == "Admin")
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

        public IActionResult EmployeeData()
        {
            try
            {
                var sessionRole = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionRole"));
                if (sessionRole.ToString() == "Employee")
                {
                    var sessionName = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionName"));
                    ViewBag.SesRole = sessionRole;
                    ViewBag.SesName = sessionName;

                    var sessionId = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("SessionId"));

                    EmployeeVM employee = null;
                    var resTask = client.GetAsync("Employees/" + sessionId);
                    resTask.Wait();

                    var result = resTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                        employee = JsonConvert.DeserializeObject<EmployeeVM>(json);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error.");
                    }
                    return View(employee);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        public JsonResult LoadEmployee()
        {
            IEnumerable<Employee> employees = null;
            var resTask = client.GetAsync("Employees");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Employee>>();
                readTask.Wait();
                employees = readTask.Result;
            }
            else
            {
                employees = Enumerable.Empty<Employee>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(employees);

        }

        public JsonResult GetById(int Id)
        {
            Employee employee = null;
            var resTask = client.GetAsync("Employees/" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                employee = JsonConvert.DeserializeObject<Employee>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(employee);

        }

        public JsonResult Insert(Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (employee.Id == 0)
            {
                var result = client.PostAsync("Employees/Create", byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Update(Employee employee, int Id)
        {
            var json = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (employee.Id == Id)
            {
                var result = client.PutAsync("Employees/" + Id, byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("Employees/" + id).Result;
            return Json(result);
        }

        public async Task<IActionResult> Excel()
        {
            List<Employee> employees = new List<Employee>();
            HttpResponseMessage resView = await client.GetAsync("Employees");
            var resultView = resView.Content.ReadAsStringAsync().Result;
            employees = JsonConvert.DeserializeObject<List<Employee>>(resultView);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");
                var currentRow = 1;
                var number = 0;
                worksheet.Cell(currentRow, 1).Value = "No";
                worksheet.Cell(currentRow, 2).Value = "Employee Id";
                worksheet.Cell(currentRow, 3).Value = "Employee Name";
                worksheet.Cell(currentRow, 4).Value = "Employee Nip";

                foreach (var emp in employees)
                {
                    currentRow++;
                    number++;
                    worksheet.Cell(currentRow, 1).Value = number;
                    worksheet.Cell(currentRow, 2).Value = emp.Id;
                    worksheet.Cell(currentRow, 3).Value = emp.Name;
                    worksheet.Cell(currentRow, 4).Value = emp.NIP;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var conten = stream.ToArray();
                    return File(conten, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Employees_Data.xlsx");
                }
            }
        }
        public ActionResult ExportPdf()
        {
            EmployeePdf employeePdf = new EmployeePdf();
            List<Employee> employees = new List<Employee>();

            var resTask = client.GetAsync("Employees");
            resTask.Wait();
            var result = resTask.Result;

            var readTask = result.Content.ReadAsAsync<List<Employee>>();
            readTask.Wait();
            employees = readTask.Result;

            byte[] abytes = employeePdf.Prepare(employees);
            return File(abytes, "application/pdf");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
