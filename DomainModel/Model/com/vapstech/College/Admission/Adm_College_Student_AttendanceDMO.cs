using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Attendance", Schema = "CLG")]
    public class Adm_College_Student_AttendanceDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSA_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ACALU_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ACSA_Att_EntryType { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public DateTime ACSA_Entry { get; set; }
        public DateTime ACSA_AttendanceDate { get; set; }
        public int ACSA_ClassHeld { get; set; }
        public string ACSA_Regular_Extra { get; set; }
        public string ACSA_SystemIP { get; set; }
        public string ACSA_NetworkIP { get; set; }
        public string ACSA_MAACAdd { get; set; }
        public bool ACSA_ActiveFlag { get; set; }
        public List<Adm_College_Student_Attendance_PeriodwiseDMO> Adm_College_Student_Attendance_PeriodwiseDMO { get; set; }
        public List<Adm_College_Student_Attendance_StudentsDMO> Adm_College_Student_Attendance_StudentsDMO { get; set; }
    }
}
