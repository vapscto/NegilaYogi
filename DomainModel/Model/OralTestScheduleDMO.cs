using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Oral_Test_Schedule")]

    public class OralTestScheduleDMO : CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAOTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int IVRMSTAUL_Id { get; set; }
        public DateTime PAOTS_EntryDate { get; set; }
        public string PAOTS_ScheduleName { get; set; }
        public DateTime? PAOTS_ScheduleDate { get; set; }
        public string PAOTS_ScheduleTime { get; set; }
        public string PAOTS_ScheduleTimeTo { get; set; }
        public string PAOTS_AM_PM { get; set; }
        public string PAOTS_Remarks { get; set; }

        public string PAOTS_Skills { get; set; }

        public string PAOTS_Superviser { get; set; }

    }
}
