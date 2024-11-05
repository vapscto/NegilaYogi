using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_Schedule_WrritenTest_Marks")]

    public class WrittenTestScheduleMarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASHWTM_Id { get; set; }
        public long PAWTS_Id { get; set; }
        public long PASWM_Id { get; set; }

    }
}
