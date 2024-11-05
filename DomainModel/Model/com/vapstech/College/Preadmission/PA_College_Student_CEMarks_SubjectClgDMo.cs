using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_CEMarks_Subject", Schema = "CLG")]
    public class PA_College_Student_CEMarks_SubjectClgDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PACSTCEMS_Id { get; set; }
        public long PACA_Id { get; set; }
        public long PAMCEXM_Id { get; set; }
        public long PAMCEXMSUB_Id { get; set; }
        public Decimal PACSTCEMS_MaxMarks { get; set; }
        public Decimal PACSTCEMS_SubjectMarks { get; set; }
        public bool PACSTCEMS_ActiveFlg { get; set; }

    }
}
