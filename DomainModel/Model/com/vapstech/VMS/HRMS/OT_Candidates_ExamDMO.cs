using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_Candidates_Exam")]
    public class OT_Candidates_ExamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long  OTCANDEX_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public DateTime OTCANDEX_Date { get; set; }
        public string OTCANDEX_TotalTime { get; set; }
        public string OTCANDEX_AnswerSheetFile { get; set; }
        public string OTCANDEX_AnswerSheetPath { get; set; }
        public long OTCANDEX_TotalQnsAnswered { get; set; }
        public long OTCANDEX_TotalCorrectAns { get; set; }
        public decimal OTCANDEX_TotalMaxMarks { get; set; }
        public decimal OTCANDEX_TotalMarks { get; set; }
        public decimal OTCANDEX_Percentage { get; set; }
        public bool OTCANDEX_ActiveFlg { get; set; }
        public long OTCANDEX_CreatedBy { get; set; }
        public DateTime OTCANDEX_CreatedDate { get; set; }
        public long OTCANDEX_UpdatedBy { get; set; }
        public DateTime OTCANDEX_UpdatedDate { get; set; }
        public List<OT_Candidates_Exam_SubjectiveAnswerDMO> OT_Candidates_Exam_SubjectiveAnswerDMO { get; set; }
        public List<OT_Candidates_Exam_AnswerDMO> OT_Candidates_Exam_AnswerDMO { get; set; }
    }
}