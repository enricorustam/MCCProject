using ASP.NetCoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<DepartmentVM> GetAll();

        Task<IEnumerable<DepartmentVM>> GetById(int Id);

        int Create(DepartmentVM department);
        int Update(DepartmentVM department, int Id);

        int Delete(int Id);
    }
}
