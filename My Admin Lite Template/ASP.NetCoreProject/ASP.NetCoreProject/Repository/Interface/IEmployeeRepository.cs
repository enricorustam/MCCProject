using ASP.NetCoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeVM>> GetAll();

        EmployeeVM GetById(int Id);

        int Create(EmployeeVM employee);
        int Update(EmployeeVM employee, int Id);

        int Delete(int Id);
    }
}
