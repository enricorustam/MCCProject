using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASP.NetCoreProject.Models;
using ASP.NetCoreProject.ViewModels;
using Client.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class FormsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44358/api/")
        };
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadForm()
        {
            IEnumerable<FormVM> formsVM = null;
            var resTask = client.GetAsync("forms");
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<FormVM>>();
                readTask.Wait();
                formsVM = readTask.Result;
            }
            else
            {
                formsVM = Enumerable.Empty<FormVM>();
                ModelState.AddModelError(string.Empty, "Server Error try after sometimes.");
            }
            return Json(formsVM);
        }

        public JsonResult GetById(int Id)
        {
            FormVM formsVM = null;
            var resTask = client.GetAsync("forms/" + Id);
            resTask.Wait();

            var result = resTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                formsVM = JsonConvert.DeserializeObject<FormVM>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
            }
            return Json(formsVM);
        }

        public JsonResult InsertAndUpdate(FormVM formsVM, int id)
        {
            try
            {
                var json = JsonConvert.SerializeObject(formsVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                if (formsVM.Id == 0)
                {
                    var result = client.PostAsync("forms", byteContent).Result;
                    return Json(result);
                }
                else if (formsVM.Id == id)
                {
                    var result = client.PutAsync("forms/" + id, byteContent).Result;
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
            var result = client.DeleteAsync("forms/" + id).Result;
            return Json(result);
        }

        //public IActionResult Excel()
        //{
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("Forms");
        //        var currentRow = 1;
        //        worksheet.Cell(currentRow, 1).Value = "name";
        //        worksheet.Cell(currentRow, 2).Value = "startDate";
        //        worksheet.Cell(currentRow, 2).Value = "endDate";
        //        worksheet.Cell(currentRow, 2).Value = "duration";
        //        worksheet.Cell(currentRow, 2).Value = "Username";

        //        foreach (var Form in formsVM)
        //        {
        //            currentRow++;
        //            worksheet.Cell(currentRow, 1).Value = forms.Id;
        //            worksheet.Cell(currentRow, 2).Value = forms.Username;
        //        }

        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var content = stream.ToArray();

        //            return File(
        //                content,
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                "users.xlsx");
        //        }
        //    }
        //}

        //public void ExportToExcel()
        //{
        //    List<FormVM> emplist = db.EmployeeInfoes.Select(x => new EmployeeViewModel
        //    {
        //        EmployeeId = x.EmployeeId,
        //        EmployeeName = x.EmployeeName,
        //        Email = x.Email,
        //        Phone = x.Phone,
        //        Experience = x.Experience
        //    }).ToList();

        //    ExcelPackage pck = new ExcelPackage();
        //    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

        //    ws.Cells["A1"].Value = "Communication";
        //    ws.Cells["B1"].Value = "Com1";

        //    ws.Cells["A2"].Value = "Report";
        //    ws.Cells["B2"].Value = "Report1";

        //    ws.Cells["A3"].Value = "Date";
        //    ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

        //    ws.Cells["A6"].Value = "EmployeeId";
        //    ws.Cells["B6"].Value = "EmployeeName";
        //    ws.Cells["C6"].Value = "Email";
        //    ws.Cells["D6"].Value = "Phone";
        //    ws.Cells["E6"].Value = "Experience";

        //    int rowStart = 7;
        //    foreach (var item in emplist)
        //    {
        //        if (item.Experience < 5)
        //        {
        //            ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("pink")));

        //        }

        //        ws.Cells[string.Format("A{0}", rowStart)].Value = item.EmployeeId;
        //        ws.Cells[string.Format("B{0}", rowStart)].Value = item.EmployeeName;
        //        ws.Cells[string.Format("C{0}", rowStart)].Value = item.Email;
        //        ws.Cells[string.Format("D{0}", rowStart)].Value = item.Phone;
        //        ws.Cells[string.Format("E{0}", rowStart)].Value = item.Experience;
        //        rowStart++;
        //    }

        //    ws.Cells["A:AZ"].AutoFitColumns();
        //    Response.Clear();
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
        //    Response.BinaryWrite(pck.GetAsByteArray());
        //    Response.End();

        //}

        //HelperAPI _api = new HelperAPI();
        //public async Task<IActionResult> Index()
        //{
        //    List<FormVM> Form = new List<FormVM>();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Forms");
        //    var result = res.Content.ReadAsStringAsync().Result;
        //    Form = JsonConvert.DeserializeObject<List<FormVM>>(result);
        //    return View(Form);
        //}

        //public async Task<IActionResult> Details(int Id)
        //{
        //    var Form = new FormVM();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Forms/" + Id.ToString());
        //    var result = res.Content.ReadAsStringAsync().Result;
        //    Form = JsonConvert.DeserializeObject<FormVM>(result.Substring(1, result.Length - 2));
        //    return View(Form);
        //}

        //public async Task<ActionResult> Create()
        //{
        //    List<Supervisor> supervisors = new List<Supervisor>();
        //    List<Department> departments = new List<Department>();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Supervisors");
        //    HttpResponseMessage res2 = await client.GetAsync("Departments");
        //    var result = res.Content.ReadAsStringAsync().Result;
        //    var result2 = res2.Content.ReadAsStringAsync().Result;
        //    supervisors = JsonConvert.DeserializeObject<List<Supervisor>>(result);
        //    departments = JsonConvert.DeserializeObject<List<Department>>(result2);
        //    var list = supervisors.Select(r => r.Id);
        //    var list2 = departments.Select(r2 => r2.Id);
        //    ViewBag.supervisor = new SelectList(list, "Id");
        //    ViewBag.department = new SelectList(list2, "Id");
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(Form Form)
        //{
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = client.PostAsJsonAsync("Forms", Form).Result;
        //    if (res.Content.ReadAsStringAsync().Result == "False")
        //    {
        //        return View();
        //    }
        //    TempData["msg"] = "<script>alert('Saved Successfully!');</script>";
        //    return RedirectToAction("Index");
        //}

        //public async Task<ActionResult> Edit(int Id)
        //{

        //    List<Supervisor> supervisors = new List<Supervisor>();
        //    List<Department> departments = new List<Department>();
        //    HttpClient clientView = _api.Initial();
        //    HttpResponseMessage resView = await clientView.GetAsync("Supervisors");
        //    HttpResponseMessage resView2 = await clientView.GetAsync("Departments");
        //    var resultView = resView.Content.ReadAsStringAsync().Result;
        //    var resultView2 = resView2.Content.ReadAsStringAsync().Result;
        //    supervisors = JsonConvert.DeserializeObject<List<Supervisor>>(resultView);
        //    departments = JsonConvert.DeserializeObject<List<Department>>(resultView2);
        //    var list = supervisors.Select(r => r.Id);
        //    var list2 = departments.Select(r2 => r2.Id);
        //    ViewBag.supervisor = new SelectList(list, "Id");
        //    ViewBag.department = new SelectList(list2, "Id");

        //    var Form = new Form();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Forms/" + Id.ToString());
        //    var result = res.Content.ReadAsStringAsync().Result;
        //    Form = JsonConvert.DeserializeObject<Form>(result.Substring(1, result.Length - 2));

        //    return View(Form);
        //}

        //[HttpPost]
        //public ActionResult Edit(Form Form, int Id)
        //{
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = client.PutAsJsonAsync("Forms/" + Id.ToString(), Form).Result;
        //    if (res.Content.ReadAsStringAsync().Result == "False")
        //    {
        //        return View();
        //    }
        //    TempData["msg"] = "<script>alert('Saved Successfully!');</script>";
        //    return RedirectToAction("Index");
        //}

        //public async Task<ActionResult> Delete(int Id)
        //{
        //    var Form = new Form();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Forms/" + Id.ToString());
        //    var result = res.Content.ReadAsStringAsync().Result;
        //    Form = JsonConvert.DeserializeObject<Form>(result.Substring(1, result.Length - 2));
        //    return View(Form);
        //}

        //[HttpPost]
        //public ActionResult DeleteSend(int Id)
        //{
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = client.DeleteAsync("Forms/" + Id.ToString()).Result;
        //    if (res.Content.ReadAsStringAsync().Result != "True")
        //    {
        //        TempData["msg"] = "<script>alert('Data failed to deleted!');</script>";
        //    }
        //    TempData["msg"] = "<script>alert('Data successfully deleted!');</script>";
        //    return RedirectToAction("Index");
        //}
    }
}