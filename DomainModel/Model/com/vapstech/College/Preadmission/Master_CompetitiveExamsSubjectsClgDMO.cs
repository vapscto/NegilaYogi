using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_Master_Competitive_Exams_Subjects", Schema = "CLG")]
    public class Master_CompetitiveExamsSubjectsClgDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMCEXMSUB_Id { get; set; }
        public long PAMCEXM_Id { get; set; }
        public string PAMCEXMSUB_SubjectName { get; set; }

        public decimal? PAMCEXMSUB_MaxMarks { get; set; }
        public bool PAMCEXMSUB_ActiveFlg { get; set; }
        public DateTime PAMCEXMSUB_CreatedDate { get; set; }
        public DateTime PAMCEXMSUB_UpdatedDate { get; set; }
    }
}
