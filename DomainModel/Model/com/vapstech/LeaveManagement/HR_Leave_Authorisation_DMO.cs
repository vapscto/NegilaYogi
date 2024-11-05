using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Leave_Authorisation")]
    public class HR_Leave_Authorisation_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRLA_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRML_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRMG_Id { get; set; }
        public string HRLA_EmailTo { get; set; }
        public string HRLA_EmailCC { get; set; }
        public long? HRLA_CreatedBy { get; set; }
        public long? HRLA_UpdatedBy { get; set; }
        
        public List<HR_Leave_Auth_OrderNo_DMO> HR_Leave_Auth_OrderNo_DMO { get; set; }

    }
}
