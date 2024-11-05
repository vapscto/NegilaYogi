using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Master_Competitive_Exams_Subjects", Schema = "CLG")]
    public class Master_CompetitiveExamsSubjectsAdmClgDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMCEXMSUB_Id { get; set; }
        public long AMCEXM_Id { get; set; }
        public string AMCEXMSUB_SubjectName { get; set; }
        public bool AMCEXMSUB_ActiveFlg { get; set; }
        public DateTime AMCEXMSUB_CreatedDate { get; set; }
        public DateTime AMCEXMSUB_UpdatedDate { get; set; }

        public decimal? AMCEXMSUB_MaxMarks { get; set; }
    }
}
