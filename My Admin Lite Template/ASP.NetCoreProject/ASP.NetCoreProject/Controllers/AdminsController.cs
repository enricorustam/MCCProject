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
    public class AdminsController : ControllerBase
    {
        IAdminRepository _adminRepository;
        public AdminsController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<AdminVM>> GetAdmins()
        {
            return await _adminRepository.GetAll();
        }

        [HttpPost]
        public IActionResult CreateAdmin(AdminVM admin)
        {
            var create = _adminRepository.Create(admin);
            if (create > 0)
            {
                return Ok(create);
            }
            return BadRequest("Not Successfully");
        }

        [HttpGet("{Id}")]
        public AdminVM GetIdAdmins(int Id)
        {
            return _adminRepository.GetById(Id);
        }

        [HttpPut("{Id}")]
        public IActionResult EditAdmin(int Id, AdminVM admin)
        {
            var edit = _adminRepository.Update(admin, Id);
            if (edit > 0)
            {
                return Ok(edit);
            }
            return BadRequest("Not Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAdmin(int Id)
        {
            var delete = _adminRepository.Delete(Id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Not Successfully");
        }
    }
}
