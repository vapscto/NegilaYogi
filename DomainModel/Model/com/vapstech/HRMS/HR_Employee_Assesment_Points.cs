using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Ass_OptionsMaster")]
    public class HR_Employee_Assesment_Points : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HR_Emp_As_Opid { get; set; }
        public long MI_Id { get; set; }
        public string HR_Emp_As_Option { get; set; }
        public long HR_Emp_As_Points { get; set; }

        public bool HR_Emp_Asspoint_ActiveFlag { get; set; }

    }
}
