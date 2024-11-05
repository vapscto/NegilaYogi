using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Student_Status_Staff")]
    public class Fee_Student_Status_StaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSSST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FSSST_OBArrearAmount { get; set; }
        public long FSSST_OBExcessAmount { get; set; }
        public long FSSST_CurrentYrCharges { get; set; }
        public long FSSST_TotalCharges {get;set;}
        public long FSSST_ConcessionAmount { get; set; }
        public long FSSST_WaivedAmount { get; set; }
        public long FSSST_ToBePaid { get; set; }
        public long FSSST_PaidAmount { get; set; }
        public long FSSST_ExcessPaidAmount { get; set; }
        public long FSSST_ExcessAdjustedAmount { get; set; }
        public long FSSST_RunningExcessAmount { get; set; }
        public long FSSST_AdjustedAmount { get; set; }
        public long FSSST_RebateAmount { get; set; }
        public decimal FSSST_FineAmount { get; set; }
        public long FSSST_RefundAmount { get; set; }
        public long FSSST_RefundAmountAdjusted { get; set; }
        public decimal FSSST_NetAmount { get; set; }
        public decimal FSSST_ChequeBounceAmount { get; set; }
        public bool FSSST_ArrearFlag { get; set; }
        public bool FSSST_RefundOverFlag { get; set; }
        public bool FSSST_ActiveFlag { get; set; }
    }
}
