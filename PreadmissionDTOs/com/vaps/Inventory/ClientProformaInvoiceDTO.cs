using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class ClientProformaInvoiceDTO
    {
        public long ISMCLTPRP_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long ISMPRINC_Id { get; set; }
        public long MI_Id { get; set; }
        public string institutioncode { get; set; }
        public long FMI_Id { get; set; }
        public decimal ISMCLTPRBOM_Qty { get; set; }
        public decimal ISMPRINC_AdvPer { get; set; }
        public decimal ISMPRINC_AdvanceAmount { get; set; }
        public long ISMCLTC_Id { get; set; }
        public Array allcompany { get; set; }
        public Array modeofpaymentlist { get; set; }
        public Array banklist { get; set; }
        public Array instlistemail { get; set; }
        public Array instlistmobile { get; set; }
        public string ISMPRINC_InstallmentName { get; set; }
        public string ISMCLTC_Name { get; set; }
        public string trans_id { get; set; }
        public string FMI_Name { get; set; }
        public long? FTI_Id { get; set; }
        public string FTI_Name { get; set; }
        public long UserId { get; set; }
        public long ISMCLTPRP_Year { get; set; }
        public string ISMCLTPRP_InstallmentName { get; set; }
        public decimal? ISMCLTPRP_BalanceAmt { get; set; }
        public decimal? ISMCLTPRP_ReceivedAmt { get; set; }
        public string paymentstatusflag { get; set; }
        public decimal? ISMCLTPRP_InstallmentAmt { get; set; }

        public string ISMPRINC_ModeOfPayment { get; set; }
        public string ISMPRINC_MOURefNo { get; set; }
        public DateTime? ISMPRINC_MOUDate { get; set; }
        public long? HRMBD_Id { get; set; }
        public string ISMPRINC_WorkOrder { get; set; }
        public string ISMPRINC_PrInviceNo { get; set; }
        public decimal ISMPRINC_TotalTaxAmount { get; set; }
        public decimal ISMPRINC_TotalAmount { get; set; }
        public string ISMPRINC_Remarks { get; set; }
        public DateTime? ISMCLTPRP_PaymentDate { get; set; }
        public bool ISMCLTPRP_ActiveFlag { get; set; }
        public long ISMCLTPRP_CreatedBy { get; set; }
        public long ISMCLTPRP_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMMPR_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long asmaY_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public string ASMAY_Year { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string msg { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array instlist { get; set; }
        public Array yearlist { get; set; }
        public Array bomlist { get; set; }
        public Array projectlist { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public Array editlistdetails { get; set; }
        public Array editlisttax { get; set; }

        public Array taxlist { get; set; }
        public Array clientlist { get; set; }
        public long cltprid { get; set; }
        public Array getinstallment { get; set; }
        public Array geteditclient { get; set; }
        public Array geteditclientprojectid { get; set; }
        public Array paymentmodedetails { get; set; }
        public Array getpaymentdetails { get; set; }
        public Array getpaymentnotificationdetails { get; set; }

        //public long prjprid { get; set; }

        // PAYMENT DETAILS DTO
        public long ISMCPPD_Id { get; set; }
        public long flag { get; set; }
        public decimal? ISMCPPD_ReceivedAmount { get; set; }
        public DateTime? ISMCPPD_ReceivedDate { get; set; }
        public long IVRMMOD_Id { get; set; }
        public string ISMCPPD_PaymentRefNo { get; set; }
        public string ISMCPPD_Remarks { get; set; }
        public DateTime? ISMCPPD_ChequeDate { get; set; }
        // CONFIGURATION DTO
        public long ISMCPC_Id { get; set; }
        public long ISMCPC_RemainderDays { get; set; }
        public string ISMCPC_FullORPartialPayment { get; set; }
        public Array getconfigloaddata { get; set; }

        // PAYMENT NOTIFICATION DTO
        public long? IVRM_MI_Id { get; set; }
        public string ISMMCLT_ClientCode { get; set; }
        public Array getreportdetails { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public itemsdto[] itemsdto { get; set; }
        public taxdto[] taxdto { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

    }

    public class taxdto
    {
        public long INVMT_Id { get; set; }
        public string INVMT_TaxName { get; set; }
        public decimal taxamount { get; set; }
        public decimal INVMIT_TaxValue { get; set; }

    }

    public class itemsdto
    {
        public long ISMPRINCD_Id { get; set; }
        public long? ISMCLTC_Id { get; set; }
        public long? ISMCLTPRMP_Id { get; set; }
        public decimal ISMPRINCD_Qty { get; set; }
        public decimal ISMPRINCD_UnitRate { get; set; }
        public decimal ISMPRINCD_Amount { get; set; }
        public string ISMPRINCD_ItemDesc { get; set; }
        public string ISMPRINCD_Remarks { get; set; }
        public string ISMPRINCD_HSNCode { get; set; }
        public string ISMPRINCD_SACCode { get; set; }

    }
}
