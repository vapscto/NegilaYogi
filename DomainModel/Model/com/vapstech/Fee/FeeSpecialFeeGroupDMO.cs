using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_SpecialFeeHead")]
    public class FeeSpecialFeeGroupDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSFH_Id { get; set; }
        public long MI_Id { get; set; }
     //   public long FMH_ID { get; set; }
        public string FMSFH_Name { get; set; }
        public bool FMSFH_ActiceFlag { get; set; }
        public long IVRMSTAUL_Id { get; set; }
        public string FMSFH_ConcessionFlag { get; set; }
    }
}
