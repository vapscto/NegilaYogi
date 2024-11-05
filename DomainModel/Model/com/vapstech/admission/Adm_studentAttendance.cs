using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Attendance")]
    public class Adm_studentAttendance:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASA_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASA_Att_Type { get; set; }
        public string ASA_Att_EntryType { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long IMP_Id { get; set; }
        public DateTime? ASA_Entry_DateTime { get; set; }
        public DateTime? ASA_FromDate { get; set; }
        public DateTime? ASA_ToDate { get; set; }
        public decimal ASA_ClassHeld { get; set; }
        public string ASA_Regular_Extra { get; set; }
        public string ASA_Network_IP { get; set; }
        public string ASA_Mac_Add { get; set; }
        public long ASALU_Id { get; set; }
        public bool ASA_Activeflag { get; set; }
        public long? ASA_CreatedBy { get; set; }
        public long? ASA_UpdatedBy { get; set; }
        public List<Adm_studentAttendanceStudents> attstudattstd { get; set; }
        public List<Adm_studentAttendanceSubjects> attstudattsubject { get; set; }
        public List<Adm_StudentAttendancePeriodwiseDMO> attstudattperiod { get; set; }
    }
}
