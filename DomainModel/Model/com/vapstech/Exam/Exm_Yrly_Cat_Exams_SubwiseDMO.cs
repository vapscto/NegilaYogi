using DomainModel.Model.com.vapstech.Exam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Yrly_Cat_Exams_Subwise", Schema = "Exm")]
    public class Exm_Yrly_Cat_Exams_SubwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EYCES_Id { get; set; }
        [ForeignKey("EYCE_Id")]
        public int EYCE_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EYCES_MarksEntryMax { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public decimal? EYCES_MinMarks { get; set; }
        public bool EYCES_SubExamFlg { get; set; }
        public bool EYCES_SubSubjectFlg { get; set; }
        public string EYCES_MarksGradeEntryFlg { get; set; }
        public bool EYCES_MarksDisplayFlg { get; set; }
        public bool EYCES_GradeDisplayFlg { get; set; }
        public bool EYCES_AplResultFlg { get; set; }
        public bool EYCES_ActiveFlg { get; set; }
        public int EYCES_SubjectOrder { get; set; }
        public List<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO> Exm_Yrly_Cat_Exams_Subwise_SubExams { get; set; }
        public List<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO> Exm_Yrly_Cat_Exams_Subwise_SubSubjects { get; set; }
        public List<Exm_Yrly_Cat_Exams_Subwise_PTDMO> Exm_Yrly_Cat_Exams_Subwise_PTDMO { get; set; }
    }
}
