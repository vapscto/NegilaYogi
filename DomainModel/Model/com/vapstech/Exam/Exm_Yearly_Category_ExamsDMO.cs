using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Yearly_Category_Exams", Schema = "Exm")]
    public class Exm_Yearly_Category_ExamsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EYCE_Id { get; set; }
       // [ForeignKey("EYC_Id")]
        public int EYC_Id { get; set; }
        public int EME_Id { get; set; }
        public int EMGR_Id { get; set; }
        public DateTime? EYCE_AttendanceFromDate { get; set; }
        public DateTime? EYCE_AttendanceToDate { get; set; }
        public bool EYCE_SubExamFlg { get; set; }
        public bool EYCE_SubSubjectFlg { get; set; }
        public bool EYCE_ActiveFlg { get; set; }
        public DateTime? EYCE_ExamStartDate { get; set; }
        public DateTime? EYCE_ExamEndDate { get; set; }
        public DateTime? EYCE_MarksEntryLastDate { get; set; }
        public DateTime? EYCE_MarksProcessLastDate { get; set; }
        public DateTime? EYCE_MarksPublishDate { get; set; }
        public bool? EYCE_BestOfApplicableFlg { get; set; }
        public long? EYCE_BestOf { get; set; }
       public List<Exm_Yrly_Cat_Exams_SubwiseDMO> Exm_Yrly_Cat_Exams_Subwise { get; set; }
    }
}
