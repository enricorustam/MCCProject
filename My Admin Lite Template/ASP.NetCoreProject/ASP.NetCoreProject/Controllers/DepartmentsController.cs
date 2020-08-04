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
    public class DepartmentsController : ControllerBase
    {
        private IDepartmentRepository _departmentRepository;
        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IEnumerable<DepartmentVM> GetDepartments()
        {
            return _departmentRepository.GetAll();
        }

        [HttpPost]
        public IActionResult CreateDepartment(DepartmentVM department)
        {
            var create = _departmentRepository.Create(department);
            if (create > 0)
            {
                return Ok(create);
            }
            return BadRequest("Not Successfully");
        }

        [HttpGet("{Id}")]
        public async Task<IEnumerable<DepartmentVM>> GetIdDepartments(int Id)
        {
            return await _departmentRepository.GetById(Id);
        }

        [HttpPut("{Id}")]
        public IActionResult EditDepartment(int Id, DepartmentVM department)
        {
            var edit = _departmentRepository.Update(department, Id);
            if (edit > 0)
            {
                return Ok(edit);
            }
            return BadRequest("Not Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int Id)
        {
            var delete = _departmentRepository.Delete(Id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Not Successfully");
        }
    }
}