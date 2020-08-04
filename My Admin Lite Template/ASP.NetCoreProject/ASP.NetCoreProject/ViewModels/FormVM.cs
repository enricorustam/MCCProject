using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.ViewModels
{
    public class FormVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }

        public int supervisorId { get; set; }
        public string supervisorName { get; set; }
        public int departmentId { get; set; }
        public string departmentName { get; set; }
    }
}
