using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_SpecialFeeHead_FeeHead")]
    public class FeeSpecialFeeGroupsGroupingDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSFHFH_Id { get; set; }
        public long FMSFH_Id { get; set; }
        public long FMH_Id { get; set; }
        public bool FMSFHFH_ActiceFlag { get; set; }
    }
}
