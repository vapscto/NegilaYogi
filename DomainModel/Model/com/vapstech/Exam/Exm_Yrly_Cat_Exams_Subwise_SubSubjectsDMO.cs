using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Yrly_Cat_Exams_Subwise_SubSubjects", Schema = "Exm")]
    public class Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EYCESSS_Id { get; set; }
        [ForeignKey("EYCES_Id")]
        public int EYCES_Id { get; set; }
        public int EMSS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EYCESSS_MaxMarks { get; set; }
        public decimal? EYCESSS_MinMarks { get; set; }
        public decimal? EYCESSS_MarksEntryMax { get; set; }
        public bool EYCESSS_ExemptedFlg { get; set; }
        public decimal? EYCESSS_ExemptedPer { get; set; }
        public bool EYCESSS_ActiveFlg { get; set; }
        public int EYCESSS_SubSubjectOrder { get; set; }
        public bool EYCESSS_MarksFlg { get; set; }
        public bool EYCESSS_GradesFlg { get; set; }
        public bool? EYCESSS_AplResultFlg { get; set; }
        // public List<Exm_Yearly_Category_Group_SubjectsDMO> Exm_Yearly_Category_Group_Subjects { get; set; }
    }
}
