using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Circulation_Parameter_Others", Schema ="LIB")]
    public class LIB_NonBook_Circulation_Parameter_OthersDMO : CommonParamDMO
    {
        [Key]
        public long LNBCPAO_Id { get; set; }
        public long LNBCPA_Id { get; set; }
        public long LMC_Id { get; set; }
        public long LNBCPAO_NoOfItems { get; set; }
        public long LNBCPAO_IssueDays { get; set; }
        public long LNBCPAO_NoOfRenewals { get; set; }
        public bool LNBCPAO_ActiveFlg { get; set; }

    }
}
