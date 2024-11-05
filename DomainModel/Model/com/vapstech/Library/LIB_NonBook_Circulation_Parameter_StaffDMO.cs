using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Circulation_Parameter_Staff", Schema ="LIB")]
    public class LIB_NonBook_Circulation_Parameter_StaffDMO : CommonParamDMO
    {
        [Key]
        public long LNBCPAST_Id { get; set; }
        public long LNBCPA_Id { get; set; }
        public long LMC_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long LNBCPAST_NoOfItems { get; set; }
        public long LNBCPAST_IssueDays { get; set; }
        public long LNBCPAST_NoOfRenewals { get; set; }
        public bool LNBCPAST_ActiveFlg { get; set; }

    }
}
