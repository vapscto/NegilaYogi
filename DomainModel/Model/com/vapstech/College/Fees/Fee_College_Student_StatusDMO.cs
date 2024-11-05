using DomainModel.Model.com.vaps.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_College_Student_Status", Schema = "CLG")]
    public class Fee_College_Student_StatusDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FCSS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FCMAS_Id { get; set; }
        public long FCSS_OBArrearAmount { get; set; }
        public long FCSS_OBExcessAmount { get; set; }
        public long FCSS_CurrentYrCharges { get; set; }
        public long FCSS_TotalCharges { get; set; }
        public long FCSS_ConcessionAmount { get; set; }
        public long FCSS_WaivedAmount { get; set; }
        public long FCSS_ToBePaid { get; set; }
        public long FCSS_PaidAmount { get; set; }
        public long FCSS_RefundableAmount { get; set; }
        public long FCSS_ExcessPaidAmount { get; set; }
        public long FCSS_ExcessAmountAdjusted { get; set; }
        public long FCSS_RunningExcessAmount { get; set; }
        public long FCSS_AdjustedAmount { get; set; }
        public long FCSS_RebateAmount { get; set; }
        public long FCSS_FineAmount { get; set; }
        public long FCSS_RefundAmount { get; set; }
        public long FCSS_RefundAmountAdjusted { get; set; }
        public long FCSS_NetAmount { get; set; }
        //public long FCSS_ChequeBounceAmount { get; set; }
        public bool FCSS_ChequeBounceFlg { get; set; }
        public bool FCSS_ArrearFlag { get; set; }
        public bool FCSS_RefundOverFlag { get; set; }
        public bool FCSS_ActiveFlag { get; set; }
        public long User_Id { get; set; }
    }
}
