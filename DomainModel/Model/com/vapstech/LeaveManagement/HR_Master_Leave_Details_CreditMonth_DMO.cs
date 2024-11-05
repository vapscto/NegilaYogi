using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_Leave_Details_CreditMonth")]
    public class HR_Master_Leave_Details_CreditMonth_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMLDCM_Id { get; set; }
        [ForeignKey("HRMLD_Id")]
        public long HRMLD_Id { get; set; }
        //public long MI_Id { get; set; }
        public string HRMLDCM_LCMonthCode { get; set; }
        public long? HRMLDCM_CreatedBy { get; set; }
        public long? HRMLDCM_UpdatedBy { get; set; }
        public long HRMLDCM_Day { get; set; }
        



    }
}
