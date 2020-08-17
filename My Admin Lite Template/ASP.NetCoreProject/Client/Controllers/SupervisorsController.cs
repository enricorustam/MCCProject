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
    public class SupervisorsController : Controller
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
        public IActionResult SupervisorData()
        {
            try
            {
                var sessionRole = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionRole"));
                if (sessionRole.ToString() == "Supervisor")
                {
                    var sessionName = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionName"));
                    ViewBag.SesRole = sessionRole;
                    ViewBag.SesName = sessionName;

                    var sessionId = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("SessionId"));

                    SupervisorVM supervisor = null;
                    var resTask = client.GetAsync("Supervisors/" + sessionId);
                    resTask.Wait();

                    var result = resTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                        supervisor = JsonConvert.DeserializeObject<SupervisorVM>(json);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error.");
                    }
                    return View(supervisor);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        public JsonResult LoadSupervisor()
        {
            IEnumerable<Supervisor> supervisors = null;
            var resTask = client.GetAsync("supervisors");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Supervisor>>();
                readTask.Wait();
                supervisors = readTask.Result;
            }
            else
            {
                supervisors = Enumerable.Empty<Supervisor>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(supervisors);

        }

        public JsonResult GetById(int Id)
        {
            Supervisor supervisors = null;
            var resTask = client.GetAsync("supervisors/" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                supervisors = JsonConvert.DeserializeObject<Supervisor>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(supervisors);

        }

        public JsonResult Insert(Supervisor supervisor)
        {
            var json = JsonConvert.SerializeObject(supervisor);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (supervisor.Id == 0)
            {
                var result = client.PostAsync("supervisors/Create", byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Update(Supervisor supervisor, int id)
        {
            var json = JsonConvert.SerializeObject(supervisor);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (supervisor.Id == id)
            {
                var result = client.PutAsync("supervisors/" + id, byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("supervisors/" + id).Result;
            return Json(result);
        }

        public async Task<IActionResult> Excel()
        {
            List<Supervisor> supervisors = new List<Supervisor>();
            HttpResponseMessage resView = await client.GetAsync("Supervisors");
            var resultView = resView.Content.ReadAsStringAsync().Result;
            supervisors = JsonConvert.DeserializeObject<List<Supervisor>>(resultView);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Supervisors");
                var currentRow = 1;
                var number = 0;
                worksheet.Cell(currentRow, 1).Value = "No";
                worksheet.Cell(currentRow, 2).Value = "Supervisor Id";
                worksheet.Cell(currentRow, 3).Value = "Supervisor Name";
                worksheet.Cell(currentRow, 4).Value = "Supervisor Password";

                foreach (var spv in supervisors)
                {
                    currentRow++;
                    number++;
                    worksheet.Cell(currentRow, 1).Value = number;
                    worksheet.Cell(currentRow, 2).Value = spv.Id;
                    worksheet.Cell(currentRow, 3).Value = spv.Name;
                    worksheet.Cell(currentRow, 4).Value = spv.Password;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var conten = stream.ToArray();
                    return File(conten, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Supervisors_Data.xlsx");
                }
            }
        }
        public ActionResult ExportPdf()
        {
            SupervisorPdf supervisorPdf = new SupervisorPdf();
            List<Supervisor> supervisors = new List<Supervisor>();

            var resTask = client.GetAsync("Supervisors");
            resTask.Wait();
            var result = resTask.Result;

            var readTask = result.Content.ReadAsAsync<List<Supervisor>>();
            readTask.Wait();
            supervisors = readTask.Result;

            byte[] abytes = supervisorPdf.Prepare(supervisors);
            return File(abytes, "application/pdf");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}