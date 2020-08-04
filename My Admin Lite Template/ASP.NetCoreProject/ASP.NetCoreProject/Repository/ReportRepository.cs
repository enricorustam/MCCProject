using ASP.NetCoreProject.Repository.Interface;
using ASP.NetCoreProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository
{
    public class ReportRepository : IReportRepository
    {
        public int Create(ReportVM department)
        {
            throw new NotImplementedException();
        }

        public int Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReportVM> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReportVM>> GetById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
