using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Master_Competitive_Exams", Schema = "CLG")]
    public class Master_Competitive_AdmExamsClgDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMCEXM_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMCEXM_CompetitiveExams { get; set; }
        public bool AMCEXM_CompulsoryFlg { get; set; }
        public bool AMCEXM_ActiveFlg { get; set; }
        public long AMCEXM_CreatedBy { get; set; }
        public long AMCEXM_UpdatedBy { get; set; }
        public DateTime AMCEXM_CreatedDate { get; set; }
        public DateTime AMCEXM_UpdatedDate { get; set; }
    }
}
