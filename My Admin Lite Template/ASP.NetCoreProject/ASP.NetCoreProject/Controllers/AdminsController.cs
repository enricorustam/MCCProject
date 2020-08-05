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
    public class AdminsController : ControllerBase
    {
        private IAdminRepositoy _adminRepository;
        public AdminsController(AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet]
        public IEnumerable<AdminVM> GetAdmins()
        {
            return _adminRepository.GetAll();
        }


        [HttpPost]
        public IActionResult CreateAdmin([FromBody]AdminVM admin)
        {
            var create = _adminRepository.Create(admin);
            if (create > 0)
            {
                return Ok(create);
            }
            return BadRequest("Create Admin is failed");

        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<AdminVM>> GetAdmins(int Id)
        {
            return await _adminRepository.GetById(Id);
        }

        [HttpPut("{id}")]
        public IActionResult EditAdmin(int Id, AdminVM admin)
        {
            var edit = _adminRepository.Update(admin, Id);

            if (edit > 0)
            {
                return Ok(edit);
            }
            return BadRequest("Edit Admin is failed");


        }

        [HttpDelete("{Id}")]

        public IActionResult DeleteAdmin(int Id)
        {
            var delete = _adminRepository.Delete(Id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Admin is failed");

        }
    }
}