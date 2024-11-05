using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Bifurcation")]
    public class TT_Bifurcation_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public string TTB_BifurcationName { get; set; }

        public int TTB_NoOfPeriods { get; set; }
        public int TTB_AllotPeriods { get; set; }
        public int TTB_RemPeriods { get; set; }
        public int TTB_ConsecutiveFlag { get; set; }
        public int TTB_NoOfConPeriods { get; set; }
        public int TTB_NoOfConDays { get; set; }
        public int TTB_BefAftApplFlag { get; set; }

        public string TTB_BefAftFalg { get; set; }
        public int TTMP_Id { get; set; }
        public string TTB_AllotedFlag { get; set; }
        public bool TTB_ActiveFlag { get; set; }

        public List<CLGBifurcationDetailsDMO> CLGBifurcationDetailsDMO { get; set; }

    }
}
