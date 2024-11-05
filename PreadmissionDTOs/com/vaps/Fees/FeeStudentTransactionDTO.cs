using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeStudentTransactionDTO
    {
        public Array getexcesspaidstudents { get; set; }
        public Array getgroupwisepaymentdetails { get; set; }
        public Array razorpaytransactionlogs { get; set; }
        public DateTime FYPPST_Settlement_Date { get; set; }
        public string FYP_DeviceFlag { get; set; }
        public long FSC_Id { get; set; }
        public long FSCI_ID { get; set; }
        public long FSCI_ConcessionAmount { get; set; }
        //multimode
        public Temp_ModeDTO[] Modes { get; set; }
        //multimode
        public Array staffdisableterms { get; set; }
        public Array catstaffdetails { get; set; }
        public Array getfeeheaddetails { get; set; }
        public string IVRMMOD_ModeOfPayment_Code { get; set; }
        public string FYPPM_TransactionId { get; set; }
        public string FYPPM_PaymentReferenceId { get; set; }
        public string FYPPM_BankName { get; set; }
        public string FYPPM_DDChequeNo { get; set; }
        public DateTime FYPPM_DDChequeDate { get; set; }
        public decimal FYPPM_TotalPaidAmount { get; set; }
        public string FYP_TransactionTypeFlag { get; set; }
        public long FYPPSD_Id { get; set; }
        public string payment_id { get; set; }
        public string FMOT_PayGatewayType { get; set; }
        public string payinfo { get; set; }
        public long? ASMST_Id { get; set; }
        public long Totalconcession { get; set; }
        public string FYP_PayModeType { get; set; }
        public string order_id { get; set; }
        public string FPGD_SubMerchantId { get; set; }
        public Array institutiondet { get; set; }
        public string AMST_PerCity { get; set; }
        public string GROUPOFFMHIDS { get; set; }
        public string splitpayinformation { get; set; }
        public string FYP_PaymentReference_Id { get; set; }
        public string AMST_Photoname { get; set; }
        public long FMC_Id { get; set; }
        public string institutionname { get; set; }
        public string FMG_CompulsoryFlag { get; set; }
        public long validationgroupid { get; set; }
        public long validationgrougidcount { get; set; }
        public Array getpendingftiids { get; set; }
        public DateTime studentdob { get; set; }
        public Array duplicatereceipt { get; set; }
        public string termname { get; set; }
        public Array termremarks { get; set; }
        public Array currpaymentdetails { get; set; }
        public decimal FTP_Fine_Amt { get; set; }
        public Array receiptformathead { get; set; }
        public string FMCC_ConcessionName { get; set; }
        public Array fillstudenttype { get; set; }
        public long AMST_Concession_Type { get; set; }
        public string mothername { get; set; }
        public Array filltotaldetails { get; set; }
        public string FMT_Name { get; set; }
        public long FMT_Id { get; set; }
        public string Paymenttype { get; set; }
        public string ftiidss { get; set; }
        public Array feeconfiglist { get; set; }
        public long FYP_Id { get; set; }
        public Array receiparraydelete { get; set; }
        public Array showstudetails { get; set; }
        public string autoreceiptflag { get; set; }
        public string ASMAY_Year { get; set; }
        public string configset { get; set; }
        public string FMH_FeeName { get; set; }
        public string FTI_Name { get; set; }
        //public long MI_ID { get; set; }
        public long FSS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long Amst_Id { get; set; }
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
        public Array fillmastergroup { get; set; }
        public Array fillyear { get; set; }
        public Array fillmasterhead { get; set; }
        public Array fillinstallment { get; set; }
        public Array alldata { get; set; }
        public Array fillstudent { get; set; }
        public Array fillstudentviewdetails { get; set; }
        public string FMG_GroupName { get; set; }
        public decimal FMA_Amount { get; set; }
        public string returnval { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public string multiplegroups { get; set; }
        public Array fillacclst { get; set; }
        public int L_Code { get; set; }
        public string L_Name { get; set; }
        public string FYP_Receipt_No { get; set; }
        public DateTime FYP_Date { get; set; }
        public string FYPcurr_Date { get; set; }
        public string FYP_Remarks { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }
        public DateTime FYP_DD_Cheque_Date { get; set; }
        public string FYP_DD_Cheque_No { get; set; }
        public string FYP_Bank_Name { get; set; }
        public string FYP_Chq_Bounce { get; set; }
        public decimal FYP_Tot_Amount { get; set; }
        public decimal FYP_Tot_Waived_Amt { get; set; }
        public decimal FYP_Tot_Fine_Amt { get; set; }
        public decimal FYP_Tot_Concession_Amt { get; set; }
        public bool transtype { get; set; }
        public string modeofpayment { get; set; }
        public string filterinitialdata { get; set; }
        public decimal FTP_Paid_Amt { get; set; }
        public decimal FTP_Concession_Amt { get; set; }
        public decimal totalamount { get; set; }
        public decimal internalftp_tobepaid_amt { get; set; }
        public decimal internalpaidamount { get; set; }
        public decimal internalftp_concession_amt { get; set; }
        public decimal internalftp_waived_Amt { get; set; }
        public decimal internalftp_fine_Amt { get; set; }
        public long internalftsf { get; set; }
        public string validationvalue { get; set; }
        public long amst_mobile { get; set; }
        public string amst_email_id { get; set; }
        public string smsmesage { get; set; }
        public string emailmessage { get; set; }
        public long ASMCL_ID { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public long rollno { get; set; }
        public string admno { get; set; }
        public string fathername { get; set; }
        //public DateTime fyp_date { get; set; }
        public long userid { get; set; }
        public string autoreceiptnovlue { get; set; }
        public long fmt_id { get; set; }
        public Array paydet { get; set; }
        public long User_Id { get; set; }
        public long topayamount { get; set; }
        public Array feeterm { get; set; }
        public Array duedetails { get; set; }
        public long dueamount { get; set; }
        public string date { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string duration { get; set; }
        public string enddate { get; set; }
        public string paid_month { get; set; }
        public string paid_date { get; set; }
        public long fsS_TotalToBePaidaddfine { get; set; }

        //added on 12/06/2017
        //public string configset { get; set; }
        public int auto_receipt_flag { get; set; }
        public int grp_count { get; set; }
        public string FGAR_PrefixName { get; set; }
        public string FGAR_SuffixName { get; set; }
        public string FGAR_Name { get; set; }
        public string auto_FYP_Receipt_No { get; set; }
        public SaveTmpDataDTO[] savetmpdata { get; set; }
        public SaveTmpDataDTO[] savetmpdataimpl { get; set; }
        public SaveTmpDataDTO[] temarray { get; set; }
        public SaveTmpDataDTO[] temp_head_list { get; set; }
        public string searchfilter { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }
        public string searchnumber { get; set; }
        public DateTime searchdate { get; set; }
        public Array searcharray { get; set; }
        public string minstall { get; set; }
        public decimal totalcharges { get; set; }
        public Array receiparraydeleteall { get; set; }
        public Array disableterms { get; set; }
        public Array filonlinepaymentgrid { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public Array transnumconfig { get; set; }
        public string automanualreceiptno { get; set; }
        public long AMSC_Id { get; set; }
        public long yearid { get; set; }
        public string grpidss { get; set; }
        public string displaymessage { get; set; }
        public Array customgroup { get; set; }
        public string radioval { get; set; }
        public Array registrationList { get; set; }
        public long PASR_Id { get; set; }
        //preadmission 
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string PASR_RegistrationNo { get; set; }
        public long pasR_MobileNo { get; set; }
        public string pasR_emailId { get; set; }
        public int? feetermorder { get; set; }
        public Array studpaidamount { get; set; }
        public int transfersettings { get; set; }
        public long roleid { get; set; }
        public string rolename { get; set; }
        public Array customgrplist { get; set; }
        public Array termlst { get; set; }
        public string FMGG_GroupName { get; set; }
        public long FMGG_Id { get; set; }
        public string fetchcustomgrplist { get; set; }
        public string fetchtermlst { get; set; }
        public tempgroupDTO_abc[] selected_list { get; set; }
        public temotermDTO[] selected_list_mobile { get; set; }
        public Array amount_list { get; set; }
        public string merchantid { get; set; }
        public long pendingamount { get; set; }
        public long fpgdid { get; set; }
        public Array finearray { get; set; }
        public string fmg_groupname { get; set; }
        public decimal FTP_Waived_Amt { get; set; }
        public Array masterinstitution { get; set; }
        public Array paidstudents { get; set; }
        public List<FeetransactionSMS> Due_Date_array { get; set; }
        public decimal concession { get; set; }
        public string FMOT_Receipt_no { get; set; }
        //radha Preadmission transaction		
        public Array preadmissionstudentlist { get; set; }
        public Array academicyrlist { get; set; }
        public string Grp_Term_flg { get; set; }
        public Array termlist { get; set; }
        public Array grouplist { get; set; }
        public Array mapped_hds_ins { get; set; }
        public long[] terms_groups { get; set; }
        public Head_Installments_DTO[] head_installments { get; set; }
        public Array receiptdetails { get; set; }
        public string Due_Date { get; set; }
        public Array feepaiddetails { get; set; }
        public string headflag { get; set; }
        //public int headorder { get; set; }		
        public int? fmt_order { get; set; }
        public Array recenocol { get; set; }
        public Array recnogrpids { get; set; }
        public long FGAR_Id { get; set; }
        public string trans_id { get; set; }
        public long FMHOT_Id { get; set; }
        public int headorder { get; set; }
        //MB
        public string FYP_ChallanNo { get; set; }
        public bool Challan_Flag { get; set; }
        public int Fine_Amt { get; set; }
        public string FMH_Flag { get; set; }
        public Array Fine_FMA_Ids { get; set; }
        public Array specialheaddetails { get; set; }
        public Array specialheadlist { get; set; }
        public Array instalspecial { get; set; }
        public string TRMR_RouteName { get; set; }
        //MB
        public string reporttype { get; set; }
        public long[] FMGG_Ids { get; set; }
        public long fineheadfmaidsaved { get; set; }
        public long TRMR_Id { get; set; }
        public string htmldata { get; set; }
        public string filePath { get; set; }
        public string FMT_Year { get; set; }
        public Array showstaticticsdetails { get; set; }
        public int FMH_Order { get; set; }
        public string dismessage { get; set; }
        public long? pasl_id { get; set; }
        public string FMC_Online_Payment_Aca_Yr_Flag { get; set; }
        public Array readterms { get; set; }
        public int ASMAY_Order { get; set; }
        public long ASYST_Id { get; set; }
        public int ASMCL_Order { get; set; }
        public string IVRMGC_OnlinePaymentCompany { get; set; }
        //online payment 
        public string Merchant { get; set; }
        public string CommonKey { get; set; }
        public string URL { get; set; }
        public string pipeSepMsg { get; set; }
        public string xmlResponse { get; set; }
        public Array yearlst { get; set; }
        public string IVRMGC_Classwise_Payment { get; set; }
        public string FPGD_PGName { get; set; }
        public string FPGD_PGActiveFlag { get; set; }
        public Array fillpaymentgateway { get; set; }
        public long FPGD_Id { get; set; }
        public string onlinepaygteway { get; set; }
        public Array yearlist { get; set; }
        public Array srkvsdetails { get; set; }
        public FeeStudentGroupMappingDTO[] savegrplst { get; set; }
        public FeeStudentGroupMappingDTO[] saveheadlst { get; set; }
        public string fyp_transaction_id { get; set; }
        public string FPGD_AuthorisationKey { get; set; }
        public string FPGD_AuthorizationHeader { get; set; }
        public long enduserid { get; set; }
        public string portalusername { get; set; }
        public Array filusername { get; set; }
        public DateTime dated { get; set; }
        public string TRMR_PickRouteName { get; set; }
        public long? TRMR_Drop_Route { get; set; }
        public string TRMR_DropRouteName { get; set; }
        public DateTime TRSR_Date { get; set; }
        public Array routedetails { get; set; }
        public string FPGD_Image { get; set; }
        public string merchantkey { get; set; }
        public string merchanturl { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public int FSWO_FineFlg { get; set; }
        public int FSWO_FullFineWaiveOffFlg { get; set; }
        public Array partialorfullpayment { get; set; }
        public int ISPAC_FullPaymentCompFlg { get; set; }
        public Array fetchmodeofpayment { get; set; }
        public long IVRMMOD_Id { get; set; }
        public string IVRMMOD_ModeOfPayment { get; set; }
        public string IVRMMOD_Flag { get; set; }
        public Array fillmastergroupforamount { get; set; }
        public int? AMST_ECSFlag { get; set; }
        public Array transactionstatus { get; set; }
        //transaction status param
        public transactionstatuss abc { get; set; }
        //transaction status param

        //Gateway params
        public string IMPG_IndustryType { get; set; }
        public string IMPG_Website { get; set; }
        public string IMPG_TransactionStatusURL { get; set; }
        public string IMPG_SettlementURL { get; set; }
        //Gateway params        
        public string TemplateString { get; set; }
        public FeeStudentTransactionDTO[] Template { get; set; }
        public FeeStudentTransactionDTO[] selectedlist_partialpayment { get; set; }
        public Array getduedates { get; set; }
        public string smsconfig { get; set; }
        public string emailconfig { get; set; }

        public Array transactionlogs { get; set; }

        public string responsestatuslogs { get; set; }
        public string error_description { get; set; }

        public Array translogresults { get; set; }

        public string receipttemplate { get; set; }
        
           public Array pagelist { get; set; }
        public string IVRMIMP_DisplayContent { get; set; }
        public string FSC_ConcessionType { get; set; }
        public string FSC_ConcessionReason { get; set; }

        public Array GroupwiseBalance { get; set; }

        public long PreASMAY_Id { get; set; }


        public DateTime Bank_Date { get; set; }
        public string Bank_No { get; set; }
        public decimal Bank_Amount { get; set; }
        public string Bank_Name { get; set; }
        public string fyppM_TransactionTypeFlag { get; set; }
        public long FYPPM_Id { get; set; }

        public long? FSS_CreatedBy { get; set; }
        public long? FSS_UpdatedBy { get; set; }
        public long? FSS_OBAsPerFY { get; set; }
        public long? FSS_CBAsPerFY { get; set; }

        public DateTime? FYPPM_ClearanceDate { get; set; }

        public long FCSPDC_Id { get; set; }
        public string FCSPDC_ChequeNo { get; set; }
        public DateTime? FCSPDC_ChequeDate { get; set; }
        public decimal FCSPDC_Amount { get; set; }
        public string FCSPDC_Status { get; set; }
        public string FCSPDC_Narration { get; set; }
        public string FMBANK_BankName { get; set; }
        public long FMBANK_Id { get; set; }
        public Array advancefee { get; set; }

        public Array Fine_FMA_Idsadvance { get; set; }

        public SaveTmpDataDTO[] saveadvancedata { get; set; }
        public string AMST_SOL { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set;}
        public Array streamdetails { get; set; }
        public string ASMST_StreamName { get; set; }


        public string paymentlink { get; set; }

        public string strdatanew { get; set; }


        public string status { get; set; }

        public Array Classlist { get; set; }
        public Array sectionlist { get; set; }

        public long ASMS_Id { get; set; }
        //public easebuzzsplitstatus[] easebuzzsplit { get; set; }

        public long easebuzzstatus { get; set; }
        public string easebuzzdata { get; set; }
        public string easebuzzenv { get; set; }

        public long rebateamount { get; set; }
        public SaveTmpDataDTO[] FMTtotal { get; set; }
        public Array rebate_amtarr { get; set; }

        public Array termwiseamount { get; set; }
        public Array groupwiseamount { get; set; }

        public long FTOT_RebateAmount { get; set; }

        public Array narrationlist { get; set; }
        public Array studwisepartialpayment { get; set; }

        public Array Readmissionfeeschecking { get; set; }

        public Array Bankname { get; set; }


        // Feedback 16-02-2024

        public long FMTY_Id { get; set; }
        public long FMQE_Id { get; set; }
        public DateTime FSSTR_FeedbackDate { get; set; }
        public string FMQE_FeedbackQuestions { get; set; }
        public string FMQE_FeedbackQRemarks { get; set; }
        public string FMTY_FeedbackTypeName { get; set; }
        public long FMQE_FQOrder { get; set; }
        public bool FMQE_ManualEntryFlg { get; set; }
        public long FMOP_Id { get; set; }
        public string FMOP_FeedbackOptions { get; set; }
        public long FMOP_FOOrder { get; set; }
        public int FMOP_OptionsValue { get; set; }
        public Array feedbackquestion { get; set; }
        public Array feedbackoption { get; set; }
    }

    public class multipleaccounts
    {
        public string account { get; set; }
        public decimal amount { get; set; }
        //   public notes[] notes { get; set; }

     //public List<notes> notes = new List<notes>();
       public Dictionary<string, object> notes = new Dictionary<string, object>();

        public string currency { get; set; }

        public string on_hold { get; set; }
    }

    //public class notes
    //{

    //    public string notes_1 { get; set; }
    //    public string notes_2 { get; set; }
    //    public string notes_3 { get; set; }
    //    public string notes_4 { get; set; }


    //}
    public class multipleaccountscashfree
    {
        public string vendor_id { get; set; }
        public long amount { get; set; }
    }
    public class tempgroupDTO_abc
    {
        public tempgroupDTO grp { get; set; }
        public temotermDTO[] trm_list { get; set; }
    }
    public class tempgroupDTO
    {
        public string FMGG_GroupName { get; set; }
        public long FMGG_Id { get; set; }
    }
    public class temotermDTO
    {
        public string FMGG_GroupName { get; set; }
        public long FMGG_Id { get; set; }
        public string FMT_Name { get; set; }
        public long FMT_Id { get; set; }
        public int? FMT_Order { get; set; }
        public bool PreAdmFlag { get; set; }
        public long FMT_Amount { get; set; }
    }
    public class feefinefmaDTO
    {
        public string FMH_FeeName { get; set; }
        public long FMA_Id { get; set; }
        public string merchantid { get; set; }
    }
    public class feepaymentreceiptno
    {
        public string feereceiptno { get; set; }
        public string feereceiptno1 { get; set; }
    }
    public class transactionstatuss
    {
        public string MID { get; set; }
        public string ORDERID { get; set; }
        public string CHECKSUMHASH { get; set; }
        public string TXNID { get; set; }
        public string BANKTXNID { get; set; }
        public string STATUS { get; set; }
        public string TXNAMOUNT { get; set; }
        public string TXNDATE { get; set; }
    }
    public class Temp_ModeDTO
    {
        public string FYPPM_TransactionTypeFlag { get; set; }
        public decimal FYPPM_TotalPaidAmount { get; set; }
        public string FYPPM_BankName { get; set; }
        public string FYPPM_DDChequeNo { get; set; }
        public DateTime FYPPM_DDChequeDate { get; set; }
        public string IVRMMOD_ModeOfPayment { get; set; }
        public string IVRMMOD_ModeOfPayment_Code { get; set; }
        public long FYPPM_Id { get; set; }
    }

    public class Translogsresults
    {
        public string payment_id { get; set; }
        public string responsestatuslogs { get; set; }
        public string error_description { get; set; }
        public string order_id { get; set; }
        public DateTime FYP_Date { get; set; }
        public long FMA_Amount { get; set; }
        public string FYP_PayModeType { get; set; }
        public string FMOT_PayGatewayType { get; set; }
      
    }
    
}
