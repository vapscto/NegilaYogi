using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Period_Staff_ClassSection")]
    public class TT_Fixing_Period_Staff_ClassSectionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long TTFPSCC_Id { get; set; }
        public long TTFPS_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTFPSCC_Days { get; set; }
        public bool TTFPSCC_ActiveFlag { get; set; }
    }
}
