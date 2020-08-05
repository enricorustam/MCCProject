using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Models
{
    [Table("TB_M_Employee")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public int NIP { get; set; }
        public string Name { get; set; }
    }
}
