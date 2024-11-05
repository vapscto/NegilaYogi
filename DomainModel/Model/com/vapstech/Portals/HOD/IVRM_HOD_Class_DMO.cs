using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.HOD
{
    [Table("IVRM_HOD_Class")]
    public class IVRM_HOD_Class_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IHODC_Id { get; set; }
        public long IHOD_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool IHODC_ActiveFlag { get; set; }
    }
}
