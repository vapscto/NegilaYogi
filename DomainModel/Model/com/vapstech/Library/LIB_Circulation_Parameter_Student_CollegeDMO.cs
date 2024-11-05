using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Circulation_Parameter_Student_College", Schema ="LIB")]
    public class LIB_Circulation_Parameter_Student_CollegeDMO : CommonParamDMO
    {
        [Key]
        public long LBCPASC_Id { get; set; }
        public long LBCPA_Id { get; set; }
        public int LBCPASC_NoOfItems { get; set; }
        public int LBCPASC_IssueDays { get; set; }
        public int LBCPASC_NoOfRenewals { get; set; }
        public bool LBCPASC_ActiveFlg { get; set; }
        public long LBCPASC_CreatedBy { get; set; }
        public long LBCPASC_UpadtedBy { get; set; }


    }
}
