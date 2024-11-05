using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class ClientInvoiceDTO
    {
        public long ISMCLTPRP_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long ISMINC_Id { get; set; }
        public long MI_Id { get; set; }
        public string institutioncode { get; set; }
        public long FMI_Id { get; set; }
        public decimal ISMCLTPRBOM_Qty { get; set; }
        public decimal ISMINC_AdvPer { get; set; }
        public decimal ISMINC_AdvanceAmount { get; set; }
        public long ISMCLTC_Id { get; set; }
        public string ISMINC_InstallmentName { get; set; }



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
        public long? HRMBD_Id { get; set; }
        public DateTime? ISMINC_MOUDate { get; set; }
        public string ISMINC_MOURefNo { get; set; }
        public string ISMINC_ModeOfPayment { get; set; }

        public string ISMINC_WorkOrder { get; set; }
        public string ISMINC_PrInviceNo { get; set; }
        public decimal ISMINC_TotalTaxAmount { get; set; }
        public decimal ISMINC_TotalAmount { get; set; }
        public string ISMINC_Remarks { get; set; }
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
        public Array allcompany { get; set; }
        public Array modeofpaymentlist { get; set; }
        public Array banklist { get; set; }
        public Array yearlist { get; set; }
        public Array instlistmobile { get; set; }
        public Array instlistemail { get; set; }
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
        public itemsdto1[] itemsdto { get; set; }
        public taxdto1[] taxdto { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public decimal? ISMINC_TotalBasicAmount { get; set; }
        public decimal? ISMINC_TotalPercentage { get; set; }
        public string esubject { get; set; }
        public string FHEAD { get; set; }
        public string Footer { get; set; }
        public string cfilepath { get; set; }
        public string ClientMail { get; set; }
    }

    public class taxdto1
    {
        public long INVMT_Id { get; set; }
        public string INVMT_TaxName { get; set; }
        public decimal taxamount { get; set; }
        public decimal INVMIT_TaxValue { get; set; }
        public string cfilepath { get; set; }
    }

    public class itemsdto1
    {
        public long ISMINCD_Id { get; set; }
        public long? ISMCLTC_Id { get; set; }
        public long? ISMCLTPRMP_Id { get; set; }
        public decimal ISMINCD_Qty { get; set; }
        public decimal ISMINCD_UnitRate { get; set; }
        public decimal ISMINCD_Amount { get; set; }
        public string ISMINCD_ItemDesc { get; set; }
        public string ISMINCD_Remarks { get; set; }
        public string ISMINCD_HSNCode { get; set; }
        public string ISMINCD_SACCode { get; set; }

    }
}

