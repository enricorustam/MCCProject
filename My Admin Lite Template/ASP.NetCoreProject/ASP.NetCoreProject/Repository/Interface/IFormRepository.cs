using ASP.NetCoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository.Interface
{
    public interface IFormRepository
    {
        Task<IEnumerable<FormVM>> GetAll();
        Task<IEnumerable<FormVM>> GetAllfromEmp(int EmpId);
        FormVM GetById(int Id);

        int Create(FormVM form);
        int Update(FormVM form, int Id);

        int Delete(int Id);
    }
}
