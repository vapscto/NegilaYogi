using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_Leave_Details_CF")]
    public class HR_Master_Leave_Details_CF_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     
        public long HRMLDCF_Id { get; set; }
        public long HRMLD_Id { get; set; }        
        public bool HRMLDCF_MaxLeaveAplFlg { get; set; }
        public int HRMLDCF_MaxLeaveCF { get; set; }
        public long? HRMLDCF_CreatedBy { get; set; }
        public long? HRMLDCF_UpdatedBy { get; set; }
        
    }
}
