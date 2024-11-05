using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Head")]
    public class FeeHeadDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public string FMH_Flag { get; set; }
        public bool FMH_RefundFlag { get; set; }
        public bool FMH_PDAFlag { get; set; }
        public bool FMH_SpecialFeeFlag { get; set; }
        public int FMH_Order { get; set; }
        public bool FMH_ActiveFlag { get; set; }
        public long user_id { get; set; }
        public long? FMH_CreatedBy { get; set; }
        public long? FMH_UpdatedBy { get; set; }
    }
}
