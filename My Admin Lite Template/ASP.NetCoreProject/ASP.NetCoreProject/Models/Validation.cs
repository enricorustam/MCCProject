using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Models
{
    [Table("TB_M_Validation")]
    public class Validation
    {
        [Key]
        public int Id { get; set; }
        public string Action { get; set; }
        public Supervisor supervisor { get; set; }
        public Form form { get; set; }
    }
}
