using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_Head")]
    public class FeeHeadClgDMO:CommonParamDMO
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
      
    }
}
