using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Circulation_Parameter_Student_College", Schema = "LIB")]
    public class LIB_NonBook_Circulation_Parameter_Student_CollegeDMO : CommonParamDMO
    {
        [Key]

        public long LNBCPASC_Id { get; set; }
        public long LNBCPA_Id { get; set; }
        public long LMC_Id { get; set; }
        public int LNBCPASC_NoOfItems { get; set; }
        public int LNBCPASC_IssueDays { get; set; }
        public int LNBCPASC_NoOfRenewals { get; set; }
        public bool LNBCPASC_ActiveFlg { get; set; }
        public long LNBCPASC_CreatedBy { get; set; }
        public long LNBCPASC_UpadtedBy { get; set; }
    }
}
