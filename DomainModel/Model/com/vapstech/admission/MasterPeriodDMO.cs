using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_Master_Period")]
    public class MasterPeriodDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMP_Id { get; set; }

        public long MI_Id { get; set; }

        public int IMP_PeriodName { get; set; }

        public int IMP_PeriodOrder { get; set; }

        // public ICollection<pagemodulemapping> modulepagemapping { get; set; }
    }
}
