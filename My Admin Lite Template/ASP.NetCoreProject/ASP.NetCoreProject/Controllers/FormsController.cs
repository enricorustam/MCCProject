using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NetCoreProject.Repository;
using ASP.NetCoreProject.Repository.Interface;
using ASP.NetCoreProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetCoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private IFormRepository _formRepository;
        public FormsController(IFormRepository FormRepository)
        {
            _formRepository = FormRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<FormVM>> GetForms()
        {
            return await _formRepository.GetAll();
        }


        [HttpPost]
        public IActionResult CreateForm([FromBody]FormVM Form)
        {
            var create = _formRepository.Create(Form);
            if (create > 0)
            {
                return Ok(create);
            }
            return BadRequest("Create Form is failed");

        }

        [HttpGet("{id}")]
        public FormVM GetIdForms(int Id)
        {
            return _formRepository.GetById(Id);
        }

        [HttpPut("{id}")]
        public IActionResult EditForm(int Id, FormVM Form)
        {
            var edit = _formRepository.Update(Form, Id);

            if (edit > 0)
            {
                return Ok(edit);
            }
            return BadRequest("Edit Form is failed");


        }

        [HttpDelete("{Id}")]

        public IActionResult DeleteForm(int Id)
        {
            var delete = _formRepository.Delete(Id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Form is failed");

        }
    }
}