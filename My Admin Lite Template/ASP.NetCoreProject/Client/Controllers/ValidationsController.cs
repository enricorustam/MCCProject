using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ASP.NetCoreProject.ViewModels;
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
            return View();
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
    }
}