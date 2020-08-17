using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASP.NetCoreProject.Models;
using Client.Pdf;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AdminsController : Controller
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

        public JsonResult LoadAdmin()
        {
            IEnumerable<Admin> admins = null;
            var resTask = client.GetAsync("Admins");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Admin>>();
                readTask.Wait();
                admins = readTask.Result;
            }
            else
            {
                admins = Enumerable.Empty<Admin>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(admins);

        }

        public JsonResult GetById(int Id)
        {
            Admin admin = null;
            var resTask = client.GetAsync("Admins/" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                admin = JsonConvert.DeserializeObject<Admin>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(admin);

        }

        public JsonResult Insert(Admin admin)
        {
            var json = JsonConvert.SerializeObject(admin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (admin.Id == 0)
            {
                var result = client.PostAsync("Admins/Create", byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Update(Admin admin, int Id)
        {
            var json = JsonConvert.SerializeObject(admin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (admin.Id == Id)
            {
                var result = client.PutAsync("Admins/" + Id, byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("Admins/" + id).Result;
            return Json(result);
        }

        public async Task<IActionResult> Excel()
        {
            List<Admin> admins = new List<Admin>();
            HttpResponseMessage resView = await client.GetAsync("Admins");
            var resultView = resView.Content.ReadAsStringAsync().Result;
            admins = JsonConvert.DeserializeObject<List<Admin>>(resultView);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Admins");
                var currentRow = 1;
                var number = 0;
                worksheet.Cell(currentRow, 1).Value = "No";
                worksheet.Cell(currentRow, 2).Value = "Admin Id";
                worksheet.Cell(currentRow, 3).Value = "Admin Username";
                worksheet.Cell(currentRow, 4).Value = "Admin Password";

                foreach (var adm in admins)
                {
                    currentRow++;
                    number++;
                    worksheet.Cell(currentRow, 1).Value = number;
                    worksheet.Cell(currentRow, 2).Value = adm.Id;
                    worksheet.Cell(currentRow, 3).Value = adm.Username;
                    worksheet.Cell(currentRow, 4).Value = adm.Password;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var conten = stream.ToArray();
                    return File(conten, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Admins_Data.xlsx");
                }
            }
        }
        public ActionResult ExportPdf()
        {
            AdminPdf adminPdf = new AdminPdf();
            List<Admin> admins = new List<Admin>();

            var resTask = client.GetAsync("Admins");
            resTask.Wait();
            var result = resTask.Result;

            var readTask = result.Content.ReadAsAsync<List<Admin>>();
            readTask.Wait();
            admins = readTask.Result;

            byte[] abytes = adminPdf.Prepare(admins);
            return File(abytes, "application/pdf");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
