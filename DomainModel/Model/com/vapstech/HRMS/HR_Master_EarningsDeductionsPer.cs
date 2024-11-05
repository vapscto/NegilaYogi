using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_EarningsDeductionsPer")]
    public class HR_Master_EarningsDeductionsPer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEDP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMED_Id { get; set; }
        public long HRMEDP_HRMED_Id { get; set; }
    }
}
