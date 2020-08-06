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
    public class SupervisorsController : ControllerBase
    {
        private ISupervisorRepository _supervisorRepository;
        public SupervisorsController(ISupervisorRepository supervisorRepository)
        {
            _supervisorRepository = supervisorRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<SupervisorVM>> GetSupervisors()
        {
            return await _supervisorRepository.GetAll();
        }


        [HttpPost]
        public IActionResult CreateSupervisor([FromBody]SupervisorVM supervisor)
        {
            var create = _supervisorRepository.Create(supervisor);
            if (create > 0)
            {
                return Ok(create);
            }
            return BadRequest("Create Supervisor is failed");

        }

        [HttpGet("{id}")]
        public SupervisorVM GetSupervisors(int Id)
        {
            return _supervisorRepository.GetById(Id);
        }

        [HttpPut("{id}")]
        public IActionResult EditSupervisor(int Id, SupervisorVM supervisor)
        {
            var edit = _supervisorRepository.Update(supervisor, Id);

            if (edit > 0)
            {
                return Ok(edit);
            }
            return BadRequest("Edit Supervisor is failed");


        }

        [HttpDelete("{Id}")]

        public IActionResult DeleteSupervisor(int Id)
        {
            var delete = _supervisorRepository.Delete(Id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Supervisor is failed");

        }
    }
}