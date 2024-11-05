using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Attendance_Students")]
    public class Adm_studentAttendanceStudents: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASAS_Id { get; set; }
        [ForeignKey("ASA_Id")]
        public long ASA_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ASA_AttendanceFlag { get; set; }
        public decimal ASA_Class_Attended { get; set; }
        public string ASA_Dailytwice_Flag { get; set; }
        public long? ASAS_CreatedBy { get; set; }
        public long? ASAS_UpdatedBy { get; set; }
        public long? ASA_CommunicationSentFlg { get; set; }
        public Adm_studentAttendance attstudent { get; set; }
    }
}
