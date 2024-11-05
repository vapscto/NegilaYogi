using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_TimeTable_College", Schema = "CLG")]
    public class Exm_TimeTable_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long EXTTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long EME_Id { get; set; }
        public long ACST_Id { get; set; }
        public long ACSS_Id { get; set; }
        public DateTime? EXTTC_FromDateTime { get; set; }
        public DateTime? EXTTC_ToDateTime { get; set; }
     
        public string EXTTC_Note { get; set; }
        public bool EXTTC_ActiveFlag { get; set; }
        public DateTime? EXTTC_CreatedDate { get; set; }
        public DateTime? EXTTC_UpdatedDate { get; set; }
        public List<Exm_TimeTable_College_SubjectsDMO> Exm_TimeTable_College_SubjectsDMO { get; set; }
    }
}
