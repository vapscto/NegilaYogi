using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_OralTest_Schedule_College_Student", Schema = "CLG")]
    public class OralTestScheduleStudentInsertClgDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAOTSCS_Id { get; set; }
        public long PAOTSC_Id { get; set; }
        public long PACA_Id { get; set; }
        public DateTime? PAOTSCS_Date { get; set; }
        public string PAOTSCS_Time { get; set; }

        public string PAOTSCS_Time_To { get; set; }

        public int PAOTSCS_StatusFlag { get; set; }
    }
}
