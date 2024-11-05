using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{

    [Table("Preadmission_WrittenTest_Schedule")]

    public class WrittenTestScheduleDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAWTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int IVRMSTAUL_Id { get; set; }
        public string PAWTS_EntryDate { get; set; }
        public string PAWTS_ScheduleName { get; set; }
        public DateTime? PAWTS_ScheduleDate { get; set; }
        public string PAWTS_ScheduleTime { get; set; }
        public string PAWTS_ScheduleTimeTo { get; set; }
        public string PAWTS_AM_PM { get; set; }
        public string PAWTS_Remarks { get; set; }
        public string PAWTS_Superviser { get; set; }

        public string PAWTS_Skills { get; set; }

    }
}
