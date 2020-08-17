using ASP.NetCoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository.Interface
{
    public interface IAdminRepository
    {
        Task<IEnumerable<AdminVM>> GetAll();
        AdminVM GetById(int Id);
        int Create(AdminVM admin);
        int Update(AdminVM admin, int Id);
        int Delete(int Id);
        AdminVM Login(AdminVM admin);
    }
}
