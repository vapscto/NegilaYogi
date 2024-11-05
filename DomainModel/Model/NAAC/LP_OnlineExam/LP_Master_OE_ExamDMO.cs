using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Exam")]
    public class LP_Master_OE_ExamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEEX_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }       
        public long ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int? EME_Id { get; set; }
        public string LPMOEEX_ExamName { get; set; }
        public string LPMOEEX_QuestionPaperDesc { get; set; }
        public long LPMOEEX_NoOfQuestion { get; set; }
        public string LPMOEEX_ExamDuration { get; set; }
        public decimal? LPMOEEX_TotalMarks { get; set; }
        public bool LPMOEEX_RandomFlg { get; set; }        
        public bool? LPMOEEX_StudentwiseFlg { get; set; }        
        public bool LPMOEEX_UploadExamPaperFlg { get; set; }       
        public string LPMOEEX_QuestionPaper { get; set; }
        public string LPMOEEX_QuestionPapeFileName { get; set; }
        public string LPMOEEX_AnswerSheet { get; set; }
        public bool? LPMOEEX_AutoPublishFlg { get; set; }
        public string LPMOEEX_AnswerPapeFileName { get; set; }
        public bool LPMOEEX_ActiveFlg { get; set; }
        public long LPMOEEX_CreatedBy { get; set; }
        public DateTime LPMOEEX_CreatedDate { get; set; }
        public long LPMOEEX_UpdatedBy { get; set; }
        public DateTime LPMOEEX_UpdatedDate { get; set; }
        public DateTime? LPMOEEX_FromDateTime { get; set; }
        public DateTime? LPMOEEX_ToDateTime { get; set; }
        public bool? LPMOEEX_NotLinkedToQnsBankFlg { get; set; }
        public bool? LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg { get; set; }
        public string LPMOEEX_Duration { get; set; }
        public string LPMOEEX_DurationFlag { get; set; }
        public List<LP_Master_OE_Exam_LevelsDMO> LP_Master_OE_Exam_LevelsDMO { get; set; }
        public List<LP_Master_OE_Exam_TopicsDMO> LP_Master_OE_Exam_TopicsDMO { get; set; }
    }
}