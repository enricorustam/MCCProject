using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NetCoreProject.Repository.Interface;
using ASP.NetCoreProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetCoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationsController : ControllerBase
    {
        private IValidationRepository _ValidationRepository;
        public ValidationsController(IValidationRepository ValidationRepository)
        {
            _ValidationRepository = ValidationRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ValidationVM>> GetValidations()
        {
            return await _ValidationRepository.GetAll();
        }


        [HttpPost]
        public IActionResult CreateValidation([FromBody]ValidationVM Validation)
        {
            var create = _ValidationRepository.Create(Validation);
            if (create > 0)
            {
                return Ok(create);
            }
            return BadRequest("Create Validation is failed");

        }

        [HttpGet("{id}")]
        public ValidationVM GetIdValidations(int Id)
        {
            return _ValidationRepository.GetById(Id);
        }

        [HttpPut("{id}")]
        public IActionResult EditValidation(int Id, ValidationVM Validation)
        {
            var edit = _ValidationRepository.Update(Validation, Id);

            if (edit > 0)
            {
                return Ok(edit);
            }
            return BadRequest("Edit Validation is failed");


        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteValidation(int Id)
        {
            var delete = _ValidationRepository.Delete(Id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Validation is failed");

        }

        [HttpGet]
        [Route("Pie")]
        public async Task<IEnumerable<ValidationVM>> GetPie()
        {
            return await _ValidationRepository.getValidationChart();
        }
    }
}