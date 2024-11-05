using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeReceiptImportStthomasDTO
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
        public Array Academicyearlist { get; set; }
        public FeeReceiptimportstthomas[] FeeReceiptimport { get; set; }
        public string FYP_Receipt_No { get; set; }

        public long FMG_Id { get; set; }
        public long FMT_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FSS_TobePaid { get; set; }

        public string Student_Name { get; set; }

        public Array Readmissionfeeschecking { get; set; }

        //Added By PraveenGouda 
        public Array receiptdelete { get; set; }
        public Array updatereceipt { get; set; }
        public DateTime fypdate { get; set; }
        public long FYP_Id { get; set; }
        public receipt[] receipt { get; set; }
    }
    public class FeeReceiptimportstthomas
    {

        public string AMST_AdmNo { get; set; }
        public string ASMAY_Year { get; set; }
        public string FYP_Date { get; set; }
        public long FYP_Tot_Amount { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }

    }

    public class receipt
    {
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long FYP_Id { get; set; }
    }




}
