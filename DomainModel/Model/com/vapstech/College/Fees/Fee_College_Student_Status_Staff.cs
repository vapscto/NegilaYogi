using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Student_Status_Staff", Schema = "CLG")]
    public class Fee_College_Student_Status_Staff 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSSST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMCAOST_Id { get; set; }
        public long FCSSST_OBArrearAmount { get; set; }
        public long FCSSST_OBExcessAmount { get; set; }
        public long FCSSST_CurrentYrCharges { get; set; }
        public long FCSSST_TotalCharges { get; set; }
        public long FCSSST_ConcessionAmount { get; set; }
        public long FCSSST_WaivedAmount { get; set; }
        public long FCSSST_ToBePaid { get; set; }
        public long FCSSST_PaidAmount { get; set; }
        public long FCSSST_ExcessPaidAmount { get; set; }
        public long FCSSST_ExcessAdjustedAmount { get; set; }
        public long FCSSST_RunningExcessAmount { get; set; }
        public long FCSSST_AdjustedAmount { get; set; }
        public long FCSSST_RebateAmount { get; set; }
        public decimal FCSSST_FineAmount { get; set; }
        public long FCSSST_RefundAmount { get; set; }
        public long FCSSST_RefundAmountAdjusted { get; set; }
        public decimal FCSSST_NetAmount { get; set; }
        public decimal FCSSST_ChequeBounceAmount { get; set; }
        public bool FCSSST_ArrearFlag { get; set; }
        public bool FCSSST_RefundOverFlag { get; set; }
        public bool FCSSST_ActiveFlag { get; set; }

        public DateTime FCSSST_CreatedDate { get; set; }
        public DateTime FCSSST_UpdatedDate { get; set; }
        public long FCSSST_CreatedBy { get; set; }
        public long FCSSST_UpdatedBy { get; set; }
    }
}
