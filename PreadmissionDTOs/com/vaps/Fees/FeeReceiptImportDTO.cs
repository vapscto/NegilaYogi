using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeReceiptImportDTO
    {
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long UserId { get; set; }

        public long failcnt { get; set; }
        public long dupcnt { get; set; }
        public long suscnt { get; set; }
        public bool returnval { get; set; }
        public Array FeeGroup { get; set; }
        public Array FeeHead { get; set; }
        public FeeReceiptimport[] FeeReceiptimport { get; set; }
        public string FYP_Receipt_No { get; set; }

        public long FMG_Id { get; set; }
    }
    public class FeeReceiptimport
    {
      
        public string AMST_AdmNo { get; set; }
        public string ASMAY_Year { get; set; }
     //   public string FYP_Receipt_No { get; set; }
        public string FYP_Bank_Name { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }
        public string FYP_DD_Cheque_No { get; set; }
        public string FMG_GroupName { get; set; }
        public string FTI_Name { get; set; }

        public string FYP_DD_Cheque_Date { get; set; }
        public string FYP_Date { get; set; }
        public long FYP_Tot_Amount { get; set; }

     

        public Feeheadimport[] Feeheadimport { get; set; }



    }


    public class Feeheadimport
    {
        public string FMH_FeeName { get; set; }
        public long Amount { get; set; }
        public long FMH_Id { get; set; }

    }
}
