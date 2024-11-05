using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Group_Grouping_Groups")]
    public class FeeGroupGroupingDMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMGGG_Id { get; set; }
        public long FMGG_Id { get; set; }
        public long FMG_Id { get; set; }

        public FeeGroupMappingDMO fgg { get; set; }
    }
}
