using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_OralTest_Schedule_College", Schema = "CLG")]
    public class OralTestScheduleClgDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAOTSC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long IVRMSTAUL_Id { get; set; }
        public DateTime PAOTSC_Date { get; set; }
    
        public string PAOTSC_ScheduleName { get; set; }
        public DateTime? PAOTSC_ScheduleDate { get; set; }
        public string PAOTSC_ScheduleFromTime { get; set; }
        public string PAOTSC_ScheduleToTime { get; set; }
        public string PAOTSC_To_AM_PM { get; set; }
        public string PAOTSC_LB_FT { get; set; }

        public string PAOTSC_LB_TT { get; set; }

        public string PAOTSC_TimePeriod { get; set; }

        public string PAOTSC_TPFlag { get; set; }

        public long PAOTSC_Strength { get; set; }

        public string PAOTSC_Remarks { get; set; }
    }
}
