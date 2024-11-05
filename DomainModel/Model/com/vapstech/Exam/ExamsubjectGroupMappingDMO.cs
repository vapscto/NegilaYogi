using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Subject_Group", Schema = "Exm")]
    public class ExamsubjectGroupMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESG_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public string ESG_SubjectGroupName { get; set; }
        public string ESG_ExamPromotionFlag { get; set; }
        public string ESG_CompulsoryFlag { get; set; }
        public decimal ESG_GroupMinMarks { get; set; }
        public decimal? ESG_GroupMaxMarks { get; set; }
        public bool ESG_ActiveFlag { get; set; }
        public List<ExamSubjectGroupMappingExamsDMO> ExamSubjectGroupMappingExamsDMO { get; set; }
        public List<ExamSubjectGroupMappingSubjectsDMO> ExamSubjectGroupMappingSubjectsDMO { get; set; }
    }
}
