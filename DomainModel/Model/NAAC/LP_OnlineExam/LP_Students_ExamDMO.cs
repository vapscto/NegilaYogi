using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Students_Exam")]
    public class LP_Students_ExamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LPSTUEX_Id { get; set; }
        public long LPMOEEX_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime? LPSTUEX_Date { get; set; }
        public string LPSTUEX_TotalTime { get; set; }
        public long LPSTUEX_TotalQnsAnswered { get; set; }
        public long LPSTUEX_TotalCorrectAns { get; set; }
        public decimal? LPSTUEX_TotalMaxMarks { get; set; }
        public decimal? LPSTUEX_TotalMarks { get; set; }
        public decimal? LPSTUEX_Percentage { get; set; }
        public bool LPSTUEX_ActiveFlg { get; set; }
        public long LPSTUEX_CreatedBy { get; set; }
        public long LPSTUEX_UpdatedBy { get; set; }
        public bool? LPSTUEX_PublishToStudent { get; set; }
        public string LPSTUEX_StaffOrStudentUploadFlag { get; set; }
        public string LPSTUEX_MergedFile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<LP_Students_Exam_AnswerDMO> LP_Students_Exam_AnswerDMO { get; set; }
        public List<LP_Students_Exam_SubjectiveAnswerDMO> LP_Students_Exam_SubjectiveAnswerDMO { get; set; }
        public List<LP_Students_Exam_AnswersheetDMO> LP_Students_Exam_AnswersheetDMO { get; set; }
        public List<LP_Students_Exam_Answersheet_StaffDMO> LP_Students_Exam_Answersheet_StaffDMO { get; set; }
    }
}