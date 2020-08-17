using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASP.NetCoreProject.Models;
using Client.Helper;
using Client.Pdf;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class DepartmentsController : Controller
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

        public JsonResult LoadDepartment()
        {
            IEnumerable<Department> departments = null;
            var resTask = client.GetAsync("departments");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Department>>();
                readTask.Wait();
                departments = readTask.Result;
            }
            else
            {
                departments = Enumerable.Empty<Department>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(departments);

        }

        public JsonResult GetById(int Id)
        {
            Department departments = null;
            var resTask = client.GetAsync("departments/" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departments = JsonConvert.DeserializeObject<Department>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(departments);

        }

        public JsonResult Insert(Department department)
        {
            var json = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (department.Id == 0)
            {
                var result = client.PostAsync("departments", byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Update(Department department, int Id)
        {
            var json = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (department.Id == Id)
            {
                var result = client.PutAsync("departments/" + Id, byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("departments/" + id).Result;
            return Json(result);
        }

        public async Task<IActionResult> Excel()
        {
            List<Department> departments = new List<Department>();
            HttpResponseMessage resView = await client.GetAsync("Departments");
            var resultView = resView.Content.ReadAsStringAsync().Result;
            departments = JsonConvert.DeserializeObject<List<Department>>(resultView);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Departments");
                var currentRow = 1;
                var number = 0;
                worksheet.Cell(currentRow, 1).Value = "No";
                worksheet.Cell(currentRow, 2).Value = "Department Id";
                worksheet.Cell(currentRow, 3).Value = "Department Name";

                foreach (var dep in departments)
                {
                    currentRow++;
                    number++;
                    worksheet.Cell(currentRow, 1).Value = number;
                    worksheet.Cell(currentRow, 2).Value = dep.Id;
                    worksheet.Cell(currentRow, 3).Value = dep.Name;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var conten = stream.ToArray();
                    return File(conten, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Departments_Data.xlsx");
                }
            }
        }
        public ActionResult ExportPdf()
        {
            DepartmentPdf departmentPdf = new DepartmentPdf();
            List<Department> departments = new List<Department>();

            var resTask = client.GetAsync("departments");
            resTask.Wait();
            var result = resTask.Result;

            var readTask = result.Content.ReadAsAsync<List<Department>>();
            readTask.Wait();
            departments = readTask.Result;

            byte[] abytes = departmentPdf.Prepare(departments);
            return File(abytes, "application/pdf");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}