using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_TC", Schema = "CLG")]
    public class CollegeStudenttctransactionDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long? ASMAY_Id { get; set; }
        public long? AMCO_Id { get; set; }
        public long? AMB_Id { get; set; }
        public long? AMSE_Id { get; set; }
        public long? ACMS_Id { get; set; }
        public long? AMCST_Id { get; set; }
        public long? IMC_Id { get; set; }
        public long? ACSTC_WorkingDays { get; set; }
        public long? ACSTC_AttendedDays { get; set; }
        public long? ACSTC_TemporaryFlag { get; set; }
        public string ACSTC_TCNO { get; set; }
        public string ACSTC_Scholarship { get; set; }
        public string ACSTC_LanguageStudied { get; set; }
        public string ACSTC_MediumOfINStruction { get; set; }
        public string ACSTC_ElectivesStudied { get; set; }
        public string ACSTC_MedicallyExam { get; set; }
        public string ACSTC_Qual_PromotionFlag { get; set; }
        public string ACSTC_Qual_Course { get; set; }
        public string ACSTC_FeePaid { get; set; }
        public string ACSTC_FeeConcession { get; set; }
        public string ACSTC_Conduct { get; set; }
        public string ACSTC_LeavingReason { get; set; }
        public string ACSTC_Result { get; set; }
        public string ACSTC_ResultDetails { get; set; }
        public string ACSTC_NCCDetails { get; set; }
        public string ACSTC_LastExamDetails { get; set; }
        public string ACSTC_ExtraActivities { get; set; }
        public string ACSTC_Remarks { get; set; }
        public string ACSTC_ActiveFlag { get; set; }
        public string Last_Course_Studied { get; set; }
        public string Caste { get; set; }
        public decimal? Fee_Due_Amnt { get; set; }
        public decimal? Library_Due_Amnt { get; set; }
        public decimal? Store_Canteen_Due { get; set; }
        public decimal? PDA_Due { get; set; }
        public DateTime? Admission_Date { get; set; }
        public DateTime? ACSTC_TCApplicationDate { get; set; }
        public DateTime? ACSTC_TCDate { get; set; }
        public DateTime? ACSTC_TCIssueDate { get; set; }
        public DateTime? ACSTC_LastAttendedDate { get; set; }
        public DateTime? ACSTC_PromotionDate { get; set; }
    }
}
