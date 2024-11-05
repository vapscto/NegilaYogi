using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_TimeTable_College_Subjects", Schema = "CLG")]
    public class Exm_TimeTable_College_SubjectsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long EXTTCS_Id { get; set; }
        public long EXTTC_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long EMSS_Id { get; set; }
        public long ECMSPAPN_Id { get; set; }
        public string EXTTCS_PaperTitle { get; set; }
        public string EXTTCS_PaperCode { get; set; }
        public DateTime? EXTTSC_Date { get; set; }
        public long EMTTSC_Id { get; set; }
        public bool EXTTSC_ActiveFlag { get; set; }
        public DateTime? EXTTCS_CreatedDate { get; set; }
        public DateTime? EXTTCS_UpdatedDate { get; set; }
        public string EXTTC_ExaminationCenter { get; set; }
        public string EXTTC_ReportingTime { get; set; }
    }
}
