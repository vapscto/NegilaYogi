using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_OralTest_Schedule_Student")]

    public class OralTestScheduleStudentInsertDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAOTSS_Id { get; set; }
        public long PAOTS_Id { get; set; }
        public long PASR_Id { get; set; }
        public DateTime? PAOTSS_Date { get; set; }
        public string PAOTSS_Time { get; set; }

        public string PAOTSS_Time_To { get; set; }

        public int PAOTSS_StatusFlag { get; set; }
    }
}
