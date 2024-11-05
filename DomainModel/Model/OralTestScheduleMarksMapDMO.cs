using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model
{
    [Table("Preadmission_Schedule_Oral_Marks")]

    public class OralTestScheduleMarksMapDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASHOM_Id { get; set; }
        public long PAOTM_Id { get; set; }
        public long PAOTS_Id { get; set; }

    }
}
