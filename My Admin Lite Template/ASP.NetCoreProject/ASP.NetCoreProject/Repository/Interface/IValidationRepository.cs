using ASP.NetCoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository.Interface
{
    public interface IValidationRepository
    {
        Task<IEnumerable<ValidationVM>> GetAll();

        ValidationVM GetById(int Id);

        int Create(ValidationVM validation);
        int Update(ValidationVM validation, int Id);

        int Delete(int Id);
        Task<IEnumerable<ValidationVM>> getValidationChart();
    }
}
