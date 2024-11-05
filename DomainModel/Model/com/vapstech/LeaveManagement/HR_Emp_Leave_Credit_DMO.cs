using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Emp_Leave_Credit")]
    public class HR_Emp_Leave_Credit_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRELC_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRML_Id { get; set; }
        public long HRMLY_Id { get; set; }
        public DateTime HRELC_Date { get; set; }
        public string HRELC_LCMonth { get; set; }
        public int HRELC_CrLeaves { get; set; }

    }
}
