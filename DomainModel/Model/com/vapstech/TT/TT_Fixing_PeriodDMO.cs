using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Fixing_Period")]
    public class TT_Fixing_PeriodDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTFP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMCL_Id { get; set; }                                                       
        public long ASMS_Id { get; set;}
        public long HRME_Id { get; set; }
        public long IMS_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTFP_AllotedFlag { get; set; }
        public bool TTFP_ActiveFlag { get; set; }
      
    }
}
