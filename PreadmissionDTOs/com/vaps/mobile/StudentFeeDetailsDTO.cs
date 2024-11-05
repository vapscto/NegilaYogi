using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class StudentFeeDetailsDTO
    {
        public class input
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
            public long FMG_Id { get; set; }
            public long FMH_Id { get; set; }
            public long FTI_Id { get; set; }
            public string date { get; set; }
            public string month { get; set; }
            public string year { get; set; }
            public string duration { get; set; }
            public long FYP_Id { get; set; }
            public long fmt_id { get; set; }
        }
        public class PaymenthistoryDTO
        {
            public long FYP_Id { get; set; }
            public Decimal FTP_Paid_Amt { get; set; }
            public DateTime FYP_Date { get; set; }
            public string FYP_Bank_Or_Cash { get; set; }
        }
        public long nextdueamount { get; set; }
        public string nextduedate { get; set; }
        public long totalpending { get; set; }
        public long totalReceipt { get; set; }
        public long totalconcession { get; set; }
        public long totalonceinacarrier { get; set; }
        public long totalanytime { get; set; }
        public Array paymenthistory { get; set; }
      
    }
}
