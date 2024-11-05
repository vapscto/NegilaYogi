using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Circulation_Parameter_Others", Schema ="LIB")]
    public class LIB_Circulation_Parameter_OthersDMO : CommonParamDMO
    {
        [Key]
        public long LBCPAO_Id { get; set; }
        public long LBCPA_Id { get; set; }
        public long LBCPAO_NoOfItems { get; set; }
        public long LBCPAO_IssueDays { get; set; }
        public long LBCPAO_NoOfRenewals { get; set; }
        public bool LBCPAO_ActiveFlg { get; set; }


    }
}
