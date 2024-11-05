using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yrly_Cat_Exams_Subwise", Schema = "CLG")]
    public class Exm_Col_Yrly_Cat_Exams_SubwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYCES_Id { get; set; }
        public long ECYCE_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal ECYCES_MarksEntryMax { get; set; }
        public decimal ECYCES_MaxMarks { get; set; }
        public decimal ECYCES_MinMarks { get; set; }
        public bool ECYCES_SubExamFlg { get; set; }
        public bool ECYCES_SubSubjectFlg { get; set; }
        public string ECYCES_MarksGradeEntryFlg { get; set; }
        public bool ECYCES_MarksDisplayFlg { get; set; }
        public bool ECYCES_GradeDisplayFlg { get; set; }
        public bool ECYCES_AplResultFlg { get; set; }
        public bool ECYCES_SubjectOrder { get; set; }
        public bool ECYCES_ActiveFlg { get; set; }
    }
}
