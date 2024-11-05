using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.MobileApp
{
    public class FeeDTO
    {
        public class getLoadData
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public bool status { get; set; }
            public Array yearclsList { get; set; }
            public Array feecurrentyear { get; set; }

        }

        public class getDetails
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public bool status { get; set; }
            public Array getfeedetails { get; set; }
            public string type { get; set; }
        }


        public class feeReceiptGetLoadData
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public bool status { get; set; }

            public Array yearlist { get; set; }
        }

        public class getReceiptDetail
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public bool status { get; set; }
            public Array recnolist { get; set; }
            public long FYP_Id { get; set; }
            public string FYP_Receipt_No { get; set; }

        }
        public class printReceipt
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public bool status { get; set; }

            public string htmldata { get; set; }
            public string minstall { get; set; }
            public string date { get; set; }
            public string FTDD_Month { get; set; }
            public string month { get; set; }
            public string duration { get; set; }
            public string year { get; set; }
            public long FMG_Id { get; set; }
            public long FMH_Id { get; set; }
            public long FTI_Id { get; set; }
            public long fmt_id { get; set; }
            public int? fmt_order { get; set; }
            public string FMT_Year { get; set; }
            public decimal FTP_Paid_Amt { get; set; }
            public decimal FTP_Concession_Amt { get; set; }
            public decimal FTP_Fine_Amt { get; set; }
            public decimal FTP_Waived_Amt { get; set; }
            public DateTime FYP_Date { get; set; }
            public DateTime FYP_DD_Cheque_Date { get; set; }
            public string FYP_Bank_Or_Cash { get; set; }
            public string FYP_DD_Cheque_No { get; set; }
            public string FYP_Bank_Name { get; set; }
            public string FYP_Remarks { get; set; }
            public string AMST_FirstName { get; set; }
            public string AMST_MiddleName { get; set; }
            public string AMST_LastName { get; set; }
            public string FMH_FeeName { get; set; }
            public string FTI_Name { get; set; }
            public string FYP_Receipt_No { get; set; }
            public string classname { get; set; }
            public string sectionname { get; set; }
            public string fathername { get; set; }
            public string mothername { get; set; }
            public string FMCC_ConcessionName { get; set; }
            public string AMST_RegistrationNo { get; set; }
            public string fyp_transaction_id { get; set; }
            public string admno { get; set; }
            public long rollno { get; set; }
            public decimal? totalcharges { get; set; }

            public long FYP_Id { get; set; }
            public long FSS_ToBePaid { get; set; }
            public Array currpaymentdetails { get; set; }
            public Array fillstudentviewdetails { get; set; }
            public Array dueamount { get; set; }


        }


        public class dueDate
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long FYP_Id { get; set; }
            public bool status { get; set; }
            public Array getduedates { get; set; }
            public Array fillstudentviewdetails { get; set; }


        }

        public class getFeetotalamount
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }

            public string configset { get; set; }

            public Array filonlinepaymentgrid { get; set; }
        }

        public class feeAnalysis        {            public long AMST_Id { get; set; }            public long MI_Id { get; set; }            public long ASMAY_Id { get; set; }            public bool status { get; set; }            public Array studentfeedetails { get; set; }            public Array feeAnalysisList { get; set; }            public long FSS_CurrentYrCharges { get; set; }            public long FSS_ToBePaid { get; set; }            public long FSS_PaidAmount { get; set; }            public long FSS_ConcessionAmount { get; set; }            public string FTI_Name { get; set; }            public string FMH_FeeName { get; set; }        }        public class feeTransactionlog        {            public long AMST_Id { get; set; }            public long MI_Id { get; set; }            public long ASMAY_Id { get; set; }            public decimal? Amount { get; set; }            public bool status { get; set; }            public string trans_id { get; set; }            public string FYP_PayModeType { get; set; }            public DateTime FYP_Date { get; set; }            public string FMOT_PayGatewayType { get; set; }            public Array translogresults { get; set; }            public Array transactionsdetails { get; set; }


        }        public class gatewayRate        {            public long MI_Id { get; set; }            public string FMOT_PayGatewayType { get; set; }            public bool status { get; set; }            public Array gatewayRatedetails { get; set; }        }
    }
}
