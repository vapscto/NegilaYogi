using DomainModel.Model.com.vaps.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Y_Payment", Schema ="CLG")]
    public class Fee_Y_PaymentDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public long FYP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string FYP_Currency { get; set; }
        public DateTime FYP_DOE { get; set; }
        public DateTime FYP_ReceiptDate { get; set; }
        public string FYP_ReceiptNo { get; set; }
        public string FYP_PayModeType { get; set; }
        public string FYP_TransactionTypeFlag { get; set; }
        public decimal FYP_TotalPaidAmount { get; set; }
        public string FYP_ChallanStatusFlag { get; set; }
        public string FYP_ChallanNo { get; set; }
        public string FYP_Transaction_Id { get; set; }
        public decimal FYP_TotalFineAmount { get; set; }
        public decimal FYP_TotalRebateAmount { get; set; }
        public string FYP_Remarks { get; set; }
        public string FYP_ChequeBounceFlag { get; set; }
        public bool FYP_ActiveFlag { get; set; }
        public long User_Id { get; set; }
        public List<Fee_Y_Payment_PaymentModeDMO> Fee_Y_Payment_PaymentModeDMO { get; set; }
        public List<Fee_Y_Payment_College_StudentDMO> Fee_Y_Payment_College_StudentDMO { get; set; }
        public List<Fee_T_College_PaymentDMO> Fee_T_College_PaymentDMO { get; set; }
        public List<Fee_Y_Payment_PA_Application> Fee_Y_Payment_PA_Application { get; set; }
        public List<Fee_T_Payment_OthStaff_College_CollegeDMO> Fee_T_Payment_OthStaff_College_CollegeDMO { get; set; }
        public List<Fee_Y_Payment_OthStu_CollegeDMO> Fee_Y_Payment_OthStu_CollegeDMO { get; set; }


        public List<Fee_Y_Payment_College_StaffDMO> Fee_Y_Payment_College_StaffDMO { get; set; }
        public bool FYP_ApprovedFlg { get; set; }
        public string FYP_PaymentReference_Id { get; set; }
        public string FYP_PayGatewayType { get; set; }
    }
}
