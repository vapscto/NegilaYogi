using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_WrittenTest_Schedule_Student")]

    public class WrittenTestScheduleStudentInsertDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PAWTSS_Id { get; set; }
        public long PAWTS_Id { get; set; }
        public long PASR_Id { get; set; }
    }
}
