using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Attendance_Students_SmartCard_Timings")]
    public class Attendance_Students_SmartCard_Timings 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASSCT_Id { get; set; }
        public long MI_Id { get; set; }
        public TimeSpan? ASSCT_FH_TimeFrom { get; set; }
        public TimeSpan? ASSCT_FH_TimeTo { get; set; }
        public TimeSpan? ASSCT_SH_TimeFrom { get; set; }
        public TimeSpan? ASSCT_SH_TimeTo { get; set; }
        public bool ASSCT_Activeflag { get; set; }
    }
}
