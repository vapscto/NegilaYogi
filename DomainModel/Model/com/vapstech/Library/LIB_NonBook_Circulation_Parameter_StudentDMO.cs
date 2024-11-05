using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Circulation_Parameter_Student", Schema ="LIB")]
    public class LIB_NonBook_Circulation_Parameter_StudentDMO : CommonParamDMO
    {
        [Key]
        public long LNBCPAS_Id { get; set; }
        public long LNBCPA_Id { get; set; }
        public long LMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long LNBCPAS_NoOfItems { get; set; }
        public long LNBCPAS_IssueDays { get; set; }
        public long LNBCPAS_NoOfRenewals { get; set; }
        public bool LNBCPAS_ActiveFlg { get; set; }


    }
}
