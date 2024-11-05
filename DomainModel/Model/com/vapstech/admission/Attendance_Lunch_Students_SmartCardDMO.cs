using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Attendance_Lunch_Students_SmartCard")]
    public class Attendance_Lunch_Students_SmartCardDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALSSC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime? ASSC_AttendanceDate { get; set; }
        public DateTime? ASSC_PunchDate { get; set; }
        public TimeSpan? ASSC_PunchTime { get; set; }
        public string ASSC_SystemIP { get; set; }
        public string ASSC_NetworkIP { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASSC_EntryForFlg { get; set; }
        public decimal ALSSC_AttendanceCount { get; set; }
    }
}
