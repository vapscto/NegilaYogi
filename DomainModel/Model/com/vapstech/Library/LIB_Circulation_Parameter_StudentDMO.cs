using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Circulation_Parameter_Student", Schema ="LIB")]
    public class LIB_Circulation_Parameter_StudentDMO : CommonParamDMO
    {
        [Key]
        public long LBCPAS_Id { get; set; }
        public long LBCPA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long LBCPAS_NoOfItems { get; set; }
        public long LBCPAS_IssueDays { get; set; }
        public long LBCPAS_NoOfRenewals { get; set; }
        public bool LBCPAS_ActiveFlg { get; set; }

    }
}
