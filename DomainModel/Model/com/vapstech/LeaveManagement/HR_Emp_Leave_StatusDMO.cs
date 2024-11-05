using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Emp_Leave_Status")]
    public class HR_Emp_Leave_StatusDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRELS_Id { get; set; }
        public long MI_Id {get; set;}
       // public long HRELS_Id {get; set;}
        public long HRME_Id {get; set;}
        public long HRML_Id {get; set;}
        public long HRMLY_Id {get; set;}
        public decimal HRELS_OBLeaves {get; set;}
        public decimal HRELS_CreditedLeaves {get; set;}
        public decimal HRELS_TotalLeaves {get; set;}     
        public decimal HRELS_TransLeaves {get; set;}
        public int HRELS_EncashedLeaves {get; set;}
        public decimal HRELS_CBLeaves {get; set;}

        public long? HRELS_CreatedBy { get; set; }
        public long? HRELS_UpdatedBy { get; set; }

    }
}
