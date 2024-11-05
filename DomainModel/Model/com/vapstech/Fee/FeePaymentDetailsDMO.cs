using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Fee;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("fee_Y_payment")]
    public class FeePaymentDetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FYP_Id { get; set; }
        //public long AMST_ID { get; set; }
        public long ASMAY_ID { get; set; }
        public long FTCU_Id { get; set; }
        public string FYP_Receipt_No { get; set; }
        public string FYP_Bank_Name { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }
        public string FYP_DD_Cheque_No { get; set; }

        // [System.ComponentModel.DefaultValue(null)]
        public DateTime FYP_DD_Cheque_Date { get; set; }
        public DateTime FYP_Date { get; set; }
        public decimal FYP_Tot_Amount { get; set; }
        public decimal FYP_Tot_Waived_Amt { get; set; }
        public decimal FYP_Tot_Fine_Amt { get; set; }
        public decimal FYP_Tot_Concession_Amt { get; set; }
        public string FYP_Remarks { get; set; }
        public long user_id { get; set; }
        public string FYP_Chq_Bounce { get; set; }
        public long MI_Id { get; set; }
        public DateTime DOE { get; set; }

        public string fyp_transaction_id { get; set; }

        public string FYP_OnlineChallanStatusFlag { get; set; }
        public string FYP_PaymentReference_Id { get; set; }
        public string FYP_ChallanNo { get; set; }
        public List<FeeTransactionPaymentDMO> ftpd { get; set; }

        public string FYP_PayModeType { get; set; }
        public string FYP_PayGatewayType { get; set; }
        public string FYP_DeviseFlg { get; set; }

        //public string fyp_transaction_id { get; set; }

        //public string FYP_OnlineChallanStatusFlag { get; set; }
        //public string FYP_PaymentReference_Id { get; set; }
        public bool? FYP_ApprovedFlg { get; set; }
        public List<Fee_T_Payment_OthStaffDMO> Fee_T_Payment_OthStaffDMO { get; set; }
        public List<Fee_Y_Payment_StaffDMO> Fee_Y_Payment_StaffDMO { get; set; }
        public List<Fee_Y_Payment_OthStuDMO> Fee_Y_Payment_OthStuDMO { get; set; }
        public List<Fee_Y_Payment_ThirdPartyDMO> Fee_Y_Payment_ThirdPartyDMO { get; set; }

        public List<Fee_Y_Payment_PaymentModeSchool> Fee_Y_Payment_PaymentModeSchool { get; set; }
        public List<FeeStudentRebate> FeeStudentRebateDMO { get; set; }

    }
}
