using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Ass_Parameter")]
    public class HR_Employee_Assesment_Parameter : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HR_Emp_As_paraid { get; set; }
        public long MI_Id { get; set; }
        public string HR_Emp_As_parameter { get; set; }
        public string HR_Emp_As_parameterdesc { get; set; }

        public bool HR_Emp_Assparameter_ActiveFlag { get; set; }

    }
}
