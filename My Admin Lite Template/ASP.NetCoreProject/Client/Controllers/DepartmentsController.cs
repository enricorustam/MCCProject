﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASP.NetCoreProject.Models;
using Client.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class DepartmentsController : Controller
    {

        //HelperAPI _api = new HelperAPI();
        //public async Task<IActionResult> Index()
        //{
        //    IEnumerable<Department> Departments;
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Departments");
        //    Departments = res.Content.ReadAsAsync<IEnumerable<Department>>().Result;
        //    return View(Departments);
        //}

        //public async Task<IActionResult> Details(int Id)
        //{
        //    var Departments = new Department();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Departments/" + Id.ToString());
        //    var result = res.Content.ReadAsStringAsync().Result;
        //    Departments = JsonConvert.DeserializeObject<Department>(result.Substring(1, result.Length - 2));
        //    return View(Departments);
        //}

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(Department Department)
        //{
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = client.PostAsJsonAsync("Departments", Department).Result;
        //    if (res.Content.ReadAsStringAsync().Result == "False")
        //    {
        //        return View();
        //    }
        //    TempData["msg"] = "<script>alert('Saved Successfully!');</script>";
        //    return RedirectToAction("Index");
        //}

        //public async Task<ActionResult> Edit(int Id)
        //{
        //    var Departments = new Department();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Departments/" + Id.ToString());
        //    var result = res.Content.ReadAsStringAsync().Result;
        //    Departments = JsonConvert.DeserializeObject<Department>(result.Substring(1, result.Length - 2));
        //    return View(Departments);
        //}

        //[HttpPost]
        //public ActionResult Edit(Department Department, int Id)
        //{
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = client.PutAsJsonAsync("Departments/" + Id.ToString(), Department).Result;
        //    if (res.Content.ReadAsStringAsync().Result == "False")
        //    {
        //        return View();
        //    }
        //    TempData["msg"] = "<script>alert('Saved Successfully!');</script>";
        //    return RedirectToAction("Index");
        //}
        //public async Task<ActionResult> Delete(int Id)
        //{
        //    var Departments = new Department();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("Departments/" + Id.ToString());
        //    var result = res.Content.ReadAsStringAsync().Result;
        //    Departments = JsonConvert.DeserializeObject<Department>(result.Substring(1, result.Length - 2));
        //    return View(Departments);
        //}

        //[HttpPost]
        //public ActionResult DeleteSend(int Id)
        //{
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = client.DeleteAsync("Departments/" + Id.ToString()).Result;
        //    if (res.Content.ReadAsStringAsync().Result != "True")
        //    {
        //        TempData["msg"] = "<script>alert('Data failed to deleted!');</script>";
        //    }
        //    TempData["msg"] = "<script>alert('Data successfully deleted!');</script>";
        //    return RedirectToAction("Index");
        //}
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44358/api/")
        };
        public IActionResult Index()
        {
            return View();
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

        public JsonResult Update(Department department, int id)
        {
            var json = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (department.Id == id)
            {
                var result = client.PutAsync("departments/" + id, byteContent).Result;
                return Json(result);
            }
            return Json(404);
        }

        public JsonResult Delete(int id)
        {
            var result = client.DeleteAsync("departments/" + id).Result;
            return Json(result);
        }
    }
}