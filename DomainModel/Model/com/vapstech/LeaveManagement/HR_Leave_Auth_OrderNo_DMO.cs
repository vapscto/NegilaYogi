using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Leave_Auth_OrderNo")]
    public class HR_Leave_Auth_OrderNo_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRLAON_Id { get; set; }
        public long HRLA_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRLAON_SanctionLevelNo { get; set; }
        public bool HRLAON_FinalFlg { get; set; }
        public long IVRMUL_Id { get; set; }
        public long? HRLAON_CreatedBy { get; set; }
        public long? HRLAON_UpdatedBy { get; set; }

        

    }
}
