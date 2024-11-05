using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Book_Circulation_Parameter", Schema ="LIB")]
    public class LIB_Book_Circulation_ParameterDMO : CommonParamDMO
    {
        [Key]
        public long LBCPA_Id { get; set; }
        public long MI_Id { get; set; }
        public string LBCPA_Flg { get; set; }
        public string LBCPA_IssueRefFlg { get; set; }
        public bool LBCPA_ActiveFlg { get; set; }
        public bool? LBCPA_ExcludeHolidayFlg { get; set; }
        public List<LIB_Circulation_Parameter_StudentDMO> LIB_Circulation_Parameter_StudentDMO { get; set; }
        public List<LIB_Circulation_Parameter_StaffDMO> lIB_Circulation_Parameter_StaffDMOs { get; set; }
        public List<LIB_Circulation_Parameter_OthersDMO> LIB_Circulation_Parameter_OthersDMO { get; set; }
        public List<LIB_Circulation_Parameter_Student_CollegeDMO> LIBCirculationParameterStudentCollegeDMO { get; set; }


    }
}
