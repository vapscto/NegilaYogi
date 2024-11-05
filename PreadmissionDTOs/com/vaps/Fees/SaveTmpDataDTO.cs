using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class SaveTmpDataDTO  
    {
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

        public float FMA_Amount { get; set; }
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
        public string FYP_Remarks { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }
        public DateTime FYP_DD_Cheque_Date { get; set; }
        public string FYP_DD_Cheque_No { get; set; }
        public string FYP_Bank_Name { get; set; }
        public string FYP_Chq_Bounce { get; set; }
       

        public decimal FYP_Tot_Amount { get; set; }
        public decimal FYP_Tot_Waived_Amt { get; set; }
        public decimal? FYP_Tot_Fine_Amt { get; set; }
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
       // public DateTime fyp_date { get; set; }

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


        public long? FMT_Total { get; set; }
        public long? Fmtidnew { get; set; }



    }


}
