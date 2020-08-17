using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASP.NetCoreProject.ViewModels;
using Client.Pdf;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class ValidationsController : Controller
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
                if ( sessionRole.ToString() == "Admin")
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

        public IActionResult OvertimeVal()
        {
            try
            {
                var sessionRole = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionRole"));
                if (sessionRole.ToString() == "Supervisor" || sessionRole.ToString() == "Employee")
                {
                    var sessionName = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionName"));
                    var sessionId = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("SessionId"));
                    ViewBag.SesRole = sessionRole;
                    ViewBag.SesName = sessionName;
                    ViewBag.SesId = sessionId;
                    return View();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        public JsonResult LoadValidation()
        {
            IEnumerable<ValidationVM> validationsVM = null;
            var resTask = client.GetAsync("validations");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<ValidationVM>>();
                readTask.Wait();
                validationsVM = readTask.Result;
            }
            else
            {
                validationsVM = Enumerable.Empty<ValidationVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(validationsVM);
        }

        public JsonResult GetById(int Id)
        {
            ValidationVM validationsVM = null;
            var resTask = client.GetAsync("validations/" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                validationsVM = JsonConvert.DeserializeObject<ValidationVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(validationsVM);
        }

        public JsonResult InsertAndUpdate(ValidationVM validationsVM, int id)
        {
            try
            {
                var json = JsonConvert.SerializeObject(validationsVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (validationsVM.Id == 0)
                {
                    var result = client.PostAsync("validations", byteContent).Result;
                    return Json(result);
                }
                else if (validationsVM.Id == id)
                {
                    var result = client.PutAsync("validations/" + id, byteContent).Result;
                    return Json(result);
                }

                return Json(404);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("validations/" + id).Result;
            return Json(result);
        }

        public async Task<IActionResult> Excel()
        {
            List<ValidationVM> validations = new List<ValidationVM>();
            HttpResponseMessage resView = await client.GetAsync("validations");
            var resultView = resView.Content.ReadAsStringAsync().Result;
            validations = JsonConvert.DeserializeObject<List<ValidationVM>>(resultView);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("validations");
                var currentRow = 1;
                var number = 0;
                worksheet.Cell(currentRow, 1).Value = "No";
                worksheet.Cell(currentRow, 2).Value = "Id";
                worksheet.Cell(currentRow, 3).Value = "Action";
                worksheet.Cell(currentRow, 4).Value = "Supervisor Name";
                worksheet.Cell(currentRow, 5).Value = "Form Id";

                foreach (var val in validations)
                {
                    currentRow++;
                    number++;
                    worksheet.Cell(currentRow, 1).Value = number;
                    worksheet.Cell(currentRow, 2).Value = val.Id;
                    worksheet.Cell(currentRow, 3).Value = val.Action;
                    worksheet.Cell(currentRow, 4).Value = val.supervisorName;
                    worksheet.Cell(currentRow, 5).Value = val.formId;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var conten = stream.ToArray();
                    return File(conten, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Validation_Data.xlsx");
                }
            }
        }

        public ActionResult ExportPdf()
        {
            ValidationPdf validationPdf = new ValidationPdf();
            List<ValidationVM> validations = new List<ValidationVM>();

            var resTask = client.GetAsync("validations");
            resTask.Wait();
            var result = resTask.Result;

            var readTask = result.Content.ReadAsAsync<List<ValidationVM>>();
            readTask.Wait();
            validations = readTask.Result;

            byte[] abytes = validationPdf.Prepare(validations);
            return File(abytes, "application/pdf");
        }
        public JsonResult LoadPieChart()
        {
            IEnumerable<ValidationVM> pieCharts = null;
            var resTask = client.GetAsync("validations/pie");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<ValidationVM>>();
                readTask.Wait();
                pieCharts = readTask.Result;
            }
            else
            {
                pieCharts = Enumerable.Empty<ValidationVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(pieCharts);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}