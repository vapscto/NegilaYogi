using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_CEMarks", Schema = "CLG")]
    public class PA_College_Student_CEMarksClgDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PACSTCEM_Id { get; set; }     
        public long PACA_Id { get; set; }
        public long PAMCEXM_Id { get; set; }
        public string PACSTCEM_RegistrationId { get; set; }
        public string PACSTCEM_RollNo { get; set; }
        public string PACSTCEM_MeritNo { get; set; }
        
        public int? PACSTCEM_ALLIndiaRank { get; set; }
        public int? PACSTCEM_CategoryRank { get; set; }
        public decimal PACSTCEM_TotalMaxMarks { get; set; }
        public decimal PACSTCEM_ObtdMarks { get; set; }
        public decimal PACSTCEM_Percentile { get; set; }
        public decimal PACSTCEM_Percentage { get; set; }
        public bool PACSTCEM_ActiveFlg { get; set; }
    }
}
