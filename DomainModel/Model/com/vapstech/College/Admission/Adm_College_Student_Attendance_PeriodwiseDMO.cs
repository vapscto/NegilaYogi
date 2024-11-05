using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Attendance_Periodwise", Schema = "CLG")]
    public class Adm_College_Student_Attendance_PeriodwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSAP_Id { get; set; }
        public long ACSA_Id { get; set; }
        public long TTMP_Id { get; set; } 
    }
}
