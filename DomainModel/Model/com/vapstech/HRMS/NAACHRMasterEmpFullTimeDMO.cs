using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("NAAC_HR_Master_EmpFullTime")]
    public class NAACHRMasterEmpFullTimeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEPT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMEPT_Year { get; set; }
        public bool HRMEPT_ActiveFlag { get; set; }
    }
}
