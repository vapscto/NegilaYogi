using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yrly_Sch_Exams_Subwise_Sub", Schema = "CLG")]
    public class Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYSESSS_Id { get; set; }
        public long ECYSES_Id { get; set; }
        public int? EMSS_Id { get; set; }
        public int? EMSE_Id { get; set; }
        public int? EMGR_Id { get; set; }
        public decimal? ECYSESSS_MaxMarks { get; set; }
        public decimal? ECYSESSS_MinMarks { get; set; }
        public bool? ECYSESSS_ExemptedFlg { get; set; }
        public decimal? ECYSESSS_ExemptedPer { get; set; }
        public int? ECYSESSS_SubSubjectOrder { get; set; }
        public bool ECYSESSS_ActiveFlg { get; set; }
        public string ECYSESSS_ProgressCardFlag { get; set; }
        public string ECYSESSS_SubjectDisplayName { get; set; }
        public string ECYSESSS_SubjectDisplayCode { get; set; }
    }
}
