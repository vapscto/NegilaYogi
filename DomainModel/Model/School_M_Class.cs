using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_School_M_Class")]
    public class School_M_Class : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_MinAgeYear { get; set; }
        public int ASMCL_MinAgeMonth { get; set; }
        public int ASMCL_MinAgeDays { get; set; }
        public int ASMCL_MaxAgeYear { get; set; }
        public int ASMCL_MaxAgeMonth { get; set; }
        public int ASMCL_MaxAgeDays { get; set; }
        public int ASMCL_Order { get; set; }
        public string ASMCL_ClassCode { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
        public int ASMCL_MaxCapacity { get; set; }
        public int ASMCL_PreadmFlag { get; set; }
    }
}
