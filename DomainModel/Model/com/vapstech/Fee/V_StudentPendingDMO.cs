using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("V_StudentPending")]
    public class V_StudentPendingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VSP_ID { get; set; }
        public long fmg_id { get; set; }
        public long fma_id { get; set; }
        public long fmh_id { get; set; }
        public long fti_id { get; set; }
        public long asmay_id { get; set; }
        public long FSS_ToBePaid { get; set; }
        public long FSS_PaidAmount { get; set; }
        public long FSS_ConcessionAmount { get; set; }
        public decimal FSS_NetAmount { get; set; }
        public decimal FSS_FineAmount { get; set; }
        public long FSS_RefundAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string FMH_FeeName { get; set; }
        public string FTI_Name { get; set; }
        public  long mi_id { get; set; }

        public long FSS_CurrentYrCharges { get; set; }
        public long FSS_TotalToBePaid { get; set; }

        public long FSS_OBArrearAmount { get; set; }
        public long FSS_RebateAmount { get; set; }
        public long FMT_Id { get; set; }

    }
}
