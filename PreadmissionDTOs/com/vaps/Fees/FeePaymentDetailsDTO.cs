using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeePaymentDetailsDTO
    {
        public long FYP_Id { get; set; }
        public long AMST_ID { get; set; }
        public long ASMAY_ID { get; set; }
        public long FTCU_Id { get; set; }
        public string FYP_Receipt_No { get; set; }
        public string FYP_Bank_Name { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }
        public long FYP_DD_Cheque_No { get; set; }

       // [System.ComponentModel.DefaultValue(null)]
        public DateTime FYP_DD_Cheque_Date { get; set; }

        public DateTime FYP_Date { get; set; }
        public decimal FYP_Tot_Amount { get; set; }
        public decimal FYP_Tot_Waived_Amt { get; set; }
        public decimal FYP_Tot_Fine_Amt { get; set; }
        public decimal FYP_Tot_Concession_Amt { get; set; }
        public string FYP_Remarks { get; set; }
        public long IVRMSTAUL_ID { get; set; }
        public char FYP_Chq_Bounce { get; set; }
    }
}
