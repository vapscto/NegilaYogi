using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Circulation_Parameter_Staff", Schema ="LIB")]
    public class LIB_Circulation_Parameter_StaffDMO : CommonParamDMO
    {
        [Key]
        public long LBCPAST_Id { get; set; }
        public long LBCPA_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long LBCPAST_NoOfItems { get; set; }
        public long LBCPAST_IssueDays { get; set; }
        public long LBCPAST_NoOfRenewals { get; set; }
        public bool LBCPAST_ActiveFlg { get; set; }


    }
}
