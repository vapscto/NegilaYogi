using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_TC")]
    public class StudentTC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long? ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public long? IMC_Id { get; set; }
        public long ASTC_ASMCL_Id { get; set; }
        public string ASTC_TCNO { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? ASTC_TCIssueDate { get; set; }
        // public DateTime ASTC_ApplicationDate { get; set; }
        public string ASTC_LeavingReason { get; set; }
        public DateTime ASTC_TCDate { get; set; }
        public DateTime ASTC_LastAttendedDate { get; set; }
        public long ASTC_WorkingDays { get; set; }
        public long ASTC_AttendedDays { get; set; }
        public DateTime? ASTC_PromotionDate { get; set; }
        public string ASTC_MediumOfINStruction { get; set; }
        public string ASTC_LanguageStudied { get; set; }
        public string ASTC_SubjectsStudied { get; set; }
        public string ASTC_ElectivesStudied { get; set; }
        public string ASTC_Scholarship { get; set; }
        public string ASTC_MedicallyExam { get; set; }
        public string ASTC_NCCDetails { get; set; }
        public string ASTC_ExtraActivities { get; set; }
        public string ASTC_Conduct { get; set; }
        public string ASTC_Remarks { get; set; }
        public string ASTC_FeePaid { get; set; }
        public string ASTC_FeeConcession { get; set; }
        public string ASTC_LastExamDetails { get; set; }
        public string ASTC_Result { get; set; }
        public string ASTC_ResultDetails { get; set; }
        public long? ASTC_TemporaryFlag { get; set; }
        public string ASTC_Qual_Class { get; set; }
        public string ASTC_Qual_PromotionFlag { get; set; }
        public string ASTC_ActiveFlag { get; set; }
        public string Last_Class_Studied { get; set; }
        public decimal? Fee_Due_Amnt { get; set; }
        public decimal? Library_Due_Amnt { get; set; }
        public decimal? Store_Canteen_Due { get; set; }
        public decimal? PDA_Due { get; set; }
        public DateTime ASTC_TCApplicationDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? ASTC_DeletedFlag { get; set; }
        public string ASTC_ReadmitRemarks { get; set; }
        public long? ASTC_CreatedBy { get; set; }
        public long? ASTC_UpdatedBy { get; set; }

    }
}
