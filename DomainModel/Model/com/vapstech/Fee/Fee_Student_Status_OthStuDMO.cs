using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Student_Status_OthStu")]
    public class Fee_Student_Status_OthStuDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSSOST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMOST_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FSSOST_OBArrearAmount { get; set; }
        public long FSSOST_OBExcessAmount { get; set; }
        public long FSSOST_CurrentYrCharges { get; set; }
        public long FSSOST_TotalCharges { get;set;}
        public long FSSOST_ConcessionAmount { get; set; }
        public long FSSOST_WaivedAmount { get; set; }
        public long FSSOST_ToBePaid { get; set; }
        public long FSSOST_PaidAmount { get; set; }
        public long FSSOST_ExcessPaidAmount { get; set; }
        public long FSSOST_ExcessAdjustedAmount { get; set; }
        public long FSSOST_RunningExcessAmount { get; set; }
        public long FSSOST_AdjustedAmount { get; set; }
        public long FSSOST_RebateAmount { get; set; }
        public decimal FSSOST_FineAmount { get; set; }
        public long FSSOST_RefundAmount { get; set; }
        public long FSSOST_RefundAmountAdjusted { get; set; }
        public decimal FSSOST_NetAmount { get; set; }
        public decimal FSSOST_ChequeBounceAmount { get; set; }
        public bool FSSOST_ArrearFlag { get; set; }
        public bool FSSOST_RefundOverFlag { get; set; }
        public bool FSSOST_ActiveFlag { get; set; }
    }
}
