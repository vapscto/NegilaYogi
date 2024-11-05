using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Attendance_LateIn_Students")]
    public class LateInStudent_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public long ALIEOS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime ALIEOS_AttendanceDate { get; set; }
        public DateTime ALIEOS_PunchDate { get; set; }
        public string ALIEOS_PunchTime { get; set; }
        public string ALIEOS_Reason { get; set; }
        public string ALIEOS_SystemIP { get; set; }
        public string ALIEOS_NetworkIP { get; set; }
        public bool ALIEOS_AactiveFlag { get; set; }

    }
}
