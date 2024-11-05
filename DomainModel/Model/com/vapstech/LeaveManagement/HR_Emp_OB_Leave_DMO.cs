using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Emp_OB_Leave")]
    public class HR_Emp_OB_Leave_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREOBL_Id { get; set; }
        public long MI_Id { get; set; }
      
        public long HRME_Id { get; set; }
       
        public long HRML_Id { get; set; }

       
        public long HRMLY_Id { get; set; }
        public DateTime? HREOBL_Date { get; set; }
        public decimal HREOBL_OBLeaves { get; set; }
        
        //public HR_Emp_Leave_Appl_DetailsDMO  HR_Emp_Leave_Appl_DetailsDMO { get; set; }
       

    }
}
