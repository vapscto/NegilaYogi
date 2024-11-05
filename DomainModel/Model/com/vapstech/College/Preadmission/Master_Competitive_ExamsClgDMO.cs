using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_Master_Competitive_Exams", Schema = "CLG")]
    public class Master_Competitive_ExamsClgDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMCEXM_Id { get; set; }
        public long MI_Id { get; set; }
        public string PAMCEXM_CompetitiveExams { get; set; }
        public bool PAMCEXM_CompulsoryFlg { get; set; }
        public bool PAMCEXM_ActiveFlg { get; set; }
        public long PAMCEXM_CreatedBy { get; set; }
        public long PAMCEXM_UpdatedBy { get; set; } 
        public DateTime PAMCEXM_CreatedDate { get; set; }
        public DateTime PAMCEXM_UpdatedDate { get; set; }
        
    }
}
