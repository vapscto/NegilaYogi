using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_RegNo_Format", Schema = "CLG")]
    public class CLGAdm_College_RegNo_FormatDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACRF_Id { get; set; }
        public long MI_Id { get; set; }
        public bool ACRF_CollegeCodeFlg { get; set; }
        public int ACRF_CCOrderFlg { get; set; }
        public bool ACRF_AYCodeFlg { get; set; }
        public int ACRF_AYCodeOrderFlg { get; set; }
        public bool ACRF_BranchCodeFlg { get; set; }
        public int ACRF_BranchCodeOrderFlg { get; set; }
        public string ACRF_NumericWidth { get; set; }
        public int ACRF_SLNo { get; set; }
        public string ACRF_StartingNo { get; set; }
        public bool ACRF_PrefilZeroFlg { get; set; }
    }
}
