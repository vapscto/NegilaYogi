using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Attendance_Subjects")]
    public class Adm_studentAttendanceSubjects: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASASU_Id { get; set; }
        [ForeignKey("ASA_Id")]
        public long ASA_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long? ASASU_CreatedBy { get; set; }
        public long? ASASU_UpdatedBy { get; set; }
        public Adm_studentAttendance attstudentsubject { get; set; }
    }
}
