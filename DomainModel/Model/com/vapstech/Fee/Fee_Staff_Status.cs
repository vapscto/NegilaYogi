using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Staff_Status")]
    public class Fee_Staff_Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FSTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_ID { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FSTS_OBArrearAmount { get; set; }
        public long FSTS_OBExcessAmount { get; set; }
        public long FSTS_CurrentYrCharges { get; set; }
        public long FSTS_TotalToBePaid { get; set; }
        public long FSTS_ToBePaid { get; set; }
        public long FSTS_PaidAmount { get; set; }
        public long FSTS_ExcessPaidAmount { get; set; }
        public long FSTS_ExcessAdjustedAmount { get; set; }
        public long FSTS_RunningExcessAmount { get; set; }

        public long FSTS_ConcessionAmount { get; set; }
        public long FSTS_AdjustedAmount { get; set; }
        public long FSTS_WaivedAmount { get; set; }
        public long FSTS_RebateAmount { get; set; }
        public decimal FSTS_FineAmount { get; set; }
        public long FSTS_RefundAmount { get; set; }

        public long FSTS_RefundAmountAdjusted { get; set; }
        public decimal FSTS_NetAmount { get; set; }
        public bool FSTS_ChequeBounceFlag { get; set; }
        public bool FSTS_ArrearFlag { get; set; }
        public bool FSTS_RefundOverFlag { get; set; }
        public bool FSTS_ActiveFlag { get; set; }

        public long User_Id { get; set; }
    }
}
