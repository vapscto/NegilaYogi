using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_CEMarks_Subject", Schema = "CLG")]
    public class Adm_College_Student_CEMarks_SubjectDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTCEMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCEXM_Id { get; set; }
        public decimal ACSTCEMS_MaxMarks { get; set; }
        public decimal ACSTCEMS_SubjectMarks { get; set; }
        public bool ACSTCEMS_ActiveFlg { get; set; }
        public long AMCEXMSUB_Id { get; set; }
    }
}
