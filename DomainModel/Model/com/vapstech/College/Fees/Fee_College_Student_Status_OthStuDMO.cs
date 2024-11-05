using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Student_Status_OthStu", Schema = "CLG")]
    public class Fee_College_Student_Status_OthStuDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSSOST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMCOST_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMCAOST_Id { get; set; }
        public long FCSSOST_OBArrearAmount { get; set; }
        public long FCSSOST_OBExcessAmount { get; set; }
        public long FCSSOST_CurrentYrCharges { get; set; }
        public long FCSSOST_TotalCharges { get; set; }
        public long FCSSOST_ConcessionAmount { get; set; }
        public long FCSSOST_WaivedAmount { get; set; }
        public long FCSSOST_ToBePaid { get; set; }
        public long FCSSOST_PaidAmount { get; set; }
        public long FCSSOST_ExcessPaidAmount { get; set; }
        public long FCSSOST_ExcessAdjustedAmount { get; set; }
        public long FCSSOST_RunningExcessAmount { get; set; }
        public long FCSSOST_AdjustedAmount { get; set; }
        public long FCSSOST_RebateAmount { get; set; }
        public decimal FCSSOST_FineAmount { get; set; }
        public long FCSSOST_RefundAmount { get; set; }
        public long FCSSOST_RefundAmountAdjusted { get; set; }
        public decimal FCSSOST_NetAmount { get; set; }
        public decimal FCSSOST_ChequeBounceAmount { get; set; }
        public bool FCSSOST_ArrearFlag { get; set; }
        public bool FCSSOST_RefundOverFlag { get; set; }
        public bool FCSSOST_ActiveFlag { get; set; }
        public DateTime? FCSSOST_CreatedDate { get; set; }
        public DateTime? FCSSOST_UpdatedDate { get; set; }
        public long FCSSOST_CreatedBy { get; set; }
        public long FCSSOST_UpdatedBy { get; set; }
    }
}
