using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_EmployeeType")]
    public class HR_Master_EmployeeType : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMET_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMET_EmployeeType { get; set; }
        public bool HRMET_ActiveFlag { get; set; }
    }
}
