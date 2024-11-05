using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Circulation_Parameter", Schema ="LIB")]
    public class LIB_NonBook_Circulation_ParameterDMO : CommonParamDMO
    {
        [Key]
        public long LNBCPA_Id { get; set; }
       public long MI_Id { get; set; }
        public string LNBCPA_Flg { get; set; }
      
        public bool LNBCPA_ActiveFlg { get; set; }

        public List<LIB_NonBook_Circulation_Parameter_OthersDMO> LIB_NonBook_Circulation_Parameter_OthersDMO { get; set; }
        public List<LIB_NonBook_Circulation_Parameter_StudentDMO> LIB_NonBook_Circulation_Parameter_StudentDMO { get; set; }
        public List<LIB_NonBook_Circulation_Parameter_StaffDMO> LIB_NonBook_Circulation_Parameter_StaffDMO { get; set; }
        public List<LIB_NonBook_Circulation_Parameter_Student_CollegeDMO> LIB_NonBook_Circulation_Parameter_Student_CollegeDMO { get; set; }

    }
}
