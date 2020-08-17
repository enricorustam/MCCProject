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
    public class EmployeesController : ControllerBase
    {
        private IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<EmployeeVM>> GetEmployees()
        {
            return await _employeeRepository.GetAll();
        }


        [HttpPost("Create")]
        public IActionResult CreateEmployee([FromBody]EmployeeVM employee)
        {
            var create = _employeeRepository.Create(employee);
            if (create > 0)
            {
                return Ok(create);
            }
            return BadRequest("Create Employee is failed");
        }

        [HttpPost("Login")]
        public EmployeeVM LoginEmployee(EmployeeVM employee)
        {
            return _employeeRepository.Login(employee);
        }

        [HttpGet("{id}")]
        public EmployeeVM GetEmployees(int Id)
        {
            return _employeeRepository.GetById(Id);
        }

        [HttpPut("{id}")]
        public IActionResult EditEmployee(int Id, EmployeeVM employee)
        {
            var edit = _employeeRepository.Update(employee, Id);

            if (edit > 0)
            {
                return Ok(edit);
            }
            return BadRequest("Edit Employee is failed");


        }

        [HttpDelete("{Id}")]

        public IActionResult DeleteEmployee(int Id)
        {
            var delete = _employeeRepository.Delete(Id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Employee is failed");

        }
    }
}