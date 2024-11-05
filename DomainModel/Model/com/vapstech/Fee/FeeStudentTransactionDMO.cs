using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    //[Table("Fee_T_Stud_FeeStatus")]
    [Table("Fee_Student_Status")]
    public class FeeStudentTransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        //public long FTSF_ID { get; set; }
        //public long Amst_Id { get; set; }
        //public long ASMCL_Id { get; set; }
        //public long fmh_id { get; set; }
        //public long fti_id { get; set; }
        //public long fmg_id { get; set; }
        //public long fma_id { get; set; }
        //public long asmay_id { get; set; }
        //public decimal ftp_tobepaid_amt { get; set; }
        //public decimal paidamount { get; set; }
        //public decimal ftp_concession_amt { get; set; }
        //public decimal ftp_waived_Amt { get; set; }
        //public decimal ftp_rebate_amt { get; set; }
        //public decimal Net_amount { get; set; }
        //public decimal ftp_fine_amt { get; set; }
        //public string reason { get; set; }
        //public string FlgArr { get; set; }
        //public float RefundAmt { get; set; }
        //public float Ftp_Chq_BON_amt { get; set; }
        //public int Ftp_Active { get; set; }
        //public float ftp_stud_ob { get; set; }
        //public string FMH_FeeName { get; set; }
        //public string FTI_Name { get; set; }
        //public long MI_ID { get; set; }


        public long FSS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FSS_OBArrearAmount { get; set; }
        public long FSS_OBExcessAmount { get; set; }
        public long FSS_CurrentYrCharges { get; set; }
        public long FSS_TotalToBePaid { get; set; }
        public long FSS_ToBePaid { get; set; }
        public long FSS_PaidAmount { get; set; }
        public long FSS_ExcessPaidAmount { get; set; }
        public long FSS_ExcessAdjustedAmount { get; set; }
        public long FSS_RunningExcessAmount { get; set; }

        public long FSS_ConcessionAmount { get; set; }
        public long FSS_AdjustedAmount { get; set; }
        public long FSS_WaivedAmount { get; set; }
        public long FSS_RebateAmount { get; set; }
        public decimal FSS_FineAmount { get; set; }
        public long FSS_RefundAmount { get; set; }

        public long FSS_RefundAmountAdjusted { get; set; }
        public decimal FSS_NetAmount { get; set; }
        public bool FSS_ChequeBounceFlag { get; set; }
        public bool FSS_ArrearFlag { get; set; }
        public bool FSS_RefundOverFlag { get; set; }
        public bool FSS_ActiveFlag { get; set; }

        public long User_Id { get; set; }

        public long FSS_RefundableAmount { get; set; }
    }
}
