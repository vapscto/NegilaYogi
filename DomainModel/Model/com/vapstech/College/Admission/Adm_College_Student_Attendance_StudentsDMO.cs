using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Attendance_Students", Schema = "CLG")]
    public class Adm_College_Student_Attendance_StudentsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSAS_Id { get; set; }
        public long ACSA_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSAS_AttendanceFlag { get; set; }
        public int ACSAS_ClassAttended { get; set; }
    }
}
