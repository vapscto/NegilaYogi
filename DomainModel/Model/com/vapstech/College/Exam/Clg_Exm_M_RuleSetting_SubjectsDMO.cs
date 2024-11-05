using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_M_RuleSetting_Subjects", Schema = "CLG")]
    public class Clg_Exm_M_RuleSetting_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EMRSS_Id { get; set; }
        public long EMRS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EMRSS_MaxMarks { get; set; }
        public decimal? EMRSS_MinMarks { get; set; }
        public decimal? EMRSS_ConvertForMarks { get; set; }
        public bool EMRSS_AppToResultFlg { get; set; }
        public bool EMRSS_ActiveFlag { get; set; }
        public List<Clg_Exm_M_RS_Subj_GroupDMO> Clg_Exm_M_RS_Subj_GroupDMO { get; set; }

    }
}
