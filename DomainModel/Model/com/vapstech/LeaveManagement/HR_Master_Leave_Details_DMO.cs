using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_Leave_Details")]
    public class HR_Master_Leave_Details_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMLD_Id { get; set; }
        public long HRML_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRMG_Id { get; set; }
        public string HRMLD_NoOfDays { get; set; }
        public int HRMLD_MaxLeaveApplicable { get; set; }
        public bool HRMLD_CarryForFlg { get; set; }
        public bool HRMLD_EncashFlg { get; set; }
        public decimal HRMLD_EncashAmount { get; set; }
        public DateTime? HRMLD_EncashOn { get; set; }
        public long? HRMLD_CreatedBy { get; set; }
        public long? HRMLD_UpdatedBy { get; set; }

        
        

        public HR_Master_Leave_DMO HR_Master_Leave_DMO { get; set; }
        public List<HR_Master_Leave_Details_CreditMonth_DMO> HR_Master_Leave_Details_CreditMonth_DMO { get; set; }
        public HR_Master_Leave_Details_CF_DMO HR_Master_Leave_Details_CF_DMO { get; set; }
        public List<HR_Master_Leave_Details_CFMonth_DMO> HR_Master_Leave_Details_CFMonth_DMO { get; set; }
        public HR_Master_Leave_Details_EnCash_DMO HR_Master_Leave_Details_EnCash_DMO { get; set; }

    }
}
