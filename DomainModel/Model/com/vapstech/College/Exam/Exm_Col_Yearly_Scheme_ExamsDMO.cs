using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yearly_Scheme_Exams", Schema = "CLG")]
    public class Exm_Col_Yearly_Scheme_ExamsDMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYSE_Id { get; set; }
        public long ECYS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public int EME_Id { get; set; }
        public int? EMGR_Id { get; set; }
        public DateTime? ECYSE_AttendanceFromDate { get; set; }
        public DateTime? ECYSE_AttendanceToDate { get; set; }
        public bool ECYSE_SubExamFlg { get; set; }
        public bool ECYSE_SubSubjectFlg { get; set; }
        public bool ECYSE_ActiveFlg { get; set; }
        public List<Exm_Col_Yrly_Sch_Exams_SubwiseDMO> Exm_Col_Yrly_Sch_Exams_SubwiseDMO { get; set; }

    }
}
