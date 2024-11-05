using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yrly_Sch_Exams_Subwise", Schema = "CLG")]
    public class Exm_Col_Yrly_Sch_Exams_SubwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYSES_Id { get; set; }
        public long ECYSE_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal ECYSES_MarksEntryMax { get; set; }
        public decimal ECYSES_MaxMarks { get; set; }
        public decimal ECYSES_MinMarks { get; set; }
        public bool ECYSES_SubExamFlg { get; set; }
        public bool ECYSES_SubSubjectFlg { get; set; }
        public string ECYSES_MarksGradeEntryFlg { get; set; }
        public bool ECYSES_MarksDisplayFlg { get; set; }
        public bool ECYSES_GradeDisplayFlg { get; set; }
        public bool ECYSES_AplResultFlg { get; set; }
        public int? ECYSES_SubjectOrder { get; set; }
        public bool ECYSES_ActiveFlg { get; set; }
        public List<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO> Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO { get; set; }
    }
}