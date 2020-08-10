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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class SupervisorsController : Controller
    {
        //    HelperAPI _api = new HelperAPI();
        //    public async Task<IActionResult> Index()
        //    {
        //        IEnumerable<Supervisor> Supervisors;
        //        HttpClient client = _api.Initial();
        //        HttpResponseMessage res = await client.GetAsync("Supervisors");
        //        Supervisors = res.Content.ReadAsAsync<IEnumerable<Supervisor>>().Result;
        //        return View(Supervisors);
        //    }

        //    public async Task<IActionResult> Details(int Id)
        //    {
        //        var Supervisors = new Supervisor();
        //        HttpClient client = _api.Initial();
        //        HttpResponseMessage res = await client.GetAsync("Supervisors/" + Id.ToString());
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        Supervisors = JsonConvert.DeserializeObject<Supervisor>(result.Substring(1, result.Length - 2));
        //        return View(Supervisors);
        //    }

        //    public ActionResult Create()
        //    {
        //        return View();
        //    }

        //    [HttpPost]
        //    public ActionResult Create(Supervisor Supervisor)
        //    {
        //        HttpClient client = _api.Initial();
        //        HttpResponseMessage res = client.PostAsJsonAsync("Supervisors", Supervisor).Result;
        //        if (res.Content.ReadAsStringAsync().Result == "False")
        //        {
        //            return View();
        //        }
        //        TempData["msg"] = "<script>alert('Saved Successfully!');</script>";
        //        return RedirectToAction("Index");
        //    }

        //    public async Task<ActionResult> Edit(int Id)
        //    {
        //        var Supervisors = new Supervisor();
        //        HttpClient client = _api.Initial();
        //        HttpResponseMessage res = await client.GetAsync("Supervisors/" + Id.ToString());
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        Supervisors = JsonConvert.DeserializeObject<Supervisor>(result.Substring(1, result.Length - 2));
        //        return View(Supervisors);
        //    }

        //    [HttpPost]
        //    public ActionResult Edit(Supervisor Supervisor, int Id)
        //    {
        //        HttpClient client = _api.Initial();
        //        HttpResponseMessage res = client.PutAsJsonAsync("Supervisors/" + Id.ToString(), Supervisor).Result;
        //        if (res.Content.ReadAsStringAsync().Result == "False")
        //        {
        //            return View();
        //        }
        //        TempData["msg"] = "<script>alert('Saved Successfully!');</script>";
        //        return RedirectToAction("Index");
        //    }
        //    public async Task<ActionResult> Delete(int Id)
        //    {
        //        var Supervisors = new Supervisor();
        //        HttpClient client = _api.Initial();
        //        HttpResponseMessage res = await client.GetAsync("Supervisors/" + Id.ToString());
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        Supervisors = JsonConvert.DeserializeObject<Supervisor>(result.Substring(1, result.Length - 2));
        //        return View(Supervisors);
        //    }

        //    [HttpPost]
        //    public ActionResult DeleteSend(int Id)
        //    {
        //        HttpClient client = _api.Initial();
        //        HttpResponseMessage res = client.DeleteAsync("Supervisors/" + Id.ToString()).Result;
        //        if (res.Content.ReadAsStringAsync().Result != "True")
        //        {
        //            TempData["msg"] = "<script>alert('Data failed to deleted!');</script>";
        //        }
        //        TempData["msg"] = "<script>alert('Data successfully deleted!');</script>";
        //        return RedirectToAction("Index");
        //    }

        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44358/api/")
        };
        public IActionResult Index()
        {
            return View();
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
                var result = client.PostAsync("supervisors", byteContent).Result;
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

                foreach (var spv in supervisors)
                {
                    currentRow++;
                    number++;
                    worksheet.Cell(currentRow, 1).Value = number;
                    worksheet.Cell(currentRow, 2).Value = spv.Id;
                    worksheet.Cell(currentRow, 3).Value = spv.Name;
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
    }
}