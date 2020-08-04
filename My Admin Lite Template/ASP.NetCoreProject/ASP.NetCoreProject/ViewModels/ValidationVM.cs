using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.ViewModels
{
    public class ValidationVM
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public int supervisorId { get; set; }
        public string supervisorName { get; set; }
        public int formId { get; set; }
        public string formName { get; set; }
    }
}
