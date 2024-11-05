using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Attendance_Periodwise")]
    public class Adm_StudentAttendancePeriodwiseDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASAP_Id { get; set; }
        [ForeignKey("ASA_Id")]
        public long ASA_Id { get; set; }
        public int TTMP_Id { get; set; }
        public long? ASAP_CreatedBy { get; set; }
        public long? ASAP_UpdatedBy { get; set; }
        public Adm_studentAttendance attstudentperiod { get; set; }
    }
}
