using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_M_Promotion", Schema = "Exm")]
    public class Exm_M_PromotionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EMP_Id { get; set; }
        public long MI_Id { get ; set; }
        public int EYC_Id { get; set; }
        public int EMGR_Id { get; set; }
        public bool EMP_PassToIndSubjectFlg { get; set; }
        public bool EMP_PassToOverallFlag { get; set; }
        public string EMP_MarksPerFlg { get; set; }
        public bool EMP_ActiveFlag { get; set; }
        public bool? EMP_BestOfApplicableFlg { get; set; }  
        public long? EMP_BestOf { get; set; }  
        public List<Exm_M_Promotion_SubjectsDMO> Exm_M_Promotion_Subjects { get; set; }
    }
}
