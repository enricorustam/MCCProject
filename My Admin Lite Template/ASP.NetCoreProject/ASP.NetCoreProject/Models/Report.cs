using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Models
{
    [Table("TB_M_Report")]
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public Validation validation { get; set; }

    }
}
