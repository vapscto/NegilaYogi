using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_Master_Period_Category_Mapping")]
    public class MasterPeriodCategoryDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMPCM_Id { get; set; }
        public long IMP_Id { get; set; }
        public long AMC_Id { get; set; }
        public string IMPCM_PeriodFlag { get; set; }

        // public ICollection<pagemodulemapping> modulepagemapping { get; set; }
    }
}
