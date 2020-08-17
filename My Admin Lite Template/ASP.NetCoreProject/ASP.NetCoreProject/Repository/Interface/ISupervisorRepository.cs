using ASP.NetCoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository.Interface
{
    public interface ISupervisorRepository
    {
        Task<IEnumerable<SupervisorVM>> GetAll();

        SupervisorVM GetById(int Id);

        int Create(SupervisorVM supervisor);
        int Update(SupervisorVM supervisor, int Id);

        int Delete(int Id);
        SupervisorVM Login(SupervisorVM employee);
    }
}
