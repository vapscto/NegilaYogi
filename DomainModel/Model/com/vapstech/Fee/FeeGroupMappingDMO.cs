using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Group_Grouping")]
    public class FeeGroupMappingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMGG_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMGG_GroupName { get; set; }
        public string FMGG_GroupCode { get; set; }
       // public long FMG_Id { get; set; }
        public bool FMGG_ActiveFlag { get; set; }

        public List<FeeGroupGroupingDMO> FeeGroupGroupingDMO { get; set; }
    }
}
