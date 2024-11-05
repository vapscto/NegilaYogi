using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_Leave_Details_CFMonth")]
    public class HR_Master_Leave_Details_CFMonth_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long HRMLDCFM_Id { get; set; }
        public long HRMLDCF_Id { get; set; }
        public int HRMLDCFM_MonthId { get; set; }
        public long? HRMLDCFM_CreatedBy { get; set; }
        public long? HRMLDCFM_UpdatedBy { get; set; }
        



    }
}
