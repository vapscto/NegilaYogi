using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_OralTest_Marks_Students")]

    public class OralTestStudentWiseMarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAOTMS_Id { get; set; }
        public long PASR_Id { get; set; }
        public long PAOTM_Id { get; set; }
        public decimal PAOTMS_Marks { get; set; }
    }
}
