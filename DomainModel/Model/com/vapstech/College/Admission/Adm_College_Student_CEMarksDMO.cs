using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_CEMarks", Schema = "CLG")]
    public class Adm_College_Student_CEMarksDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTCEM_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCEXM_Id { get; set; }
        public string ACSTCEM_RegistrationId { get; set; }
        public string ACSTCEM_RollNo { get; set; }
        public string ACSTCEM_MeritNo { get; set; }
        public decimal ACSTCEM_TotalMaxMarks { get; set; }
        public decimal ACSTCEM_ObtdMarks { get; set; }
        public int ACSTCEM_ALLIndiaRank { get; set; }
        public int ACSTCEM_CategoryRank { get; set; }
        public decimal ACSTCEM_Percentage { get; set; }
        public decimal ACSTCEM_Percentile { get; set; }
        public bool ACSTCEM_ActiveFlg { get; set; }
    }
}
