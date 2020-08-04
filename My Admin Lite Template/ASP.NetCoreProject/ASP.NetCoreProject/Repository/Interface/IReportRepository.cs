using ASP.NetCoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository.Interface
{
    public interface IReportRepository
    {
        IEnumerable<ReportVM> GetAll();

        Task<IEnumerable<ReportVM>> GetById(int Id);

        int Create(ReportVM department);
        //int Update(ReportVM department, int Id);

        int Delete(int Id);
    }
}
