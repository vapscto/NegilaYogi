using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_M_RuleSetting", Schema = "CLG")]
    public class Clg_Exm_M_RuleSettingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EMRS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ECYS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public string EMRS_MarksPerFlg { get; set; }
        public bool EMRS_ActiveFlag { get; set; }
        public List<Clg_Exm_M_RuleSetting_SubjectsDMO> Clg_Exm_M_RuleSetting_SubjectsDMO { get; set; }
    }
}
