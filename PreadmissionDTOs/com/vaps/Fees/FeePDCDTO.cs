using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeePDCDTO
    {
        public long MI_Id { get; set; }

        public long user_id { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array GroupHeadData { get; set; }
        public Array academicdrp { get; set; }

        public bool returnval { get; set; }
        public bool dupr { get; set; }
        public string message { get; set; }
        public long FMTRS_Id { get; set; }
        public long FMT_Id { get; set; }
        public long ASMAY_Id { get; set; }
       
        public string ASMAY_Year { get; set; }
      
        public Array termlist { get; set; }
        public Array fillclass { get; set; }
        public Array fillsection { get; set; }
        public Array FillBank { get; set; }

        public string FMT_Name { get; set; }
        public string AMST_EmailId { get; set; }

        public long FPDC_Id { get; set; }
      
        public long AMST_Id { get; set; }
 
        public string FPDC_ChequeNo { get; set; }
        public DateTime FPDC_ChequeDate { get; set; }
        public decimal FCSPDC_Amount { get; set; }
        public long FMBANK_Id { get; set; }
        public string FPDC_Currency { get; set; }
        public string FPDC_Narration { get; set; }
        public string FPDC_Status { get; set; }
        public bool FPDC_ActiveFlg { get; set; }
        public DateTime? FPDC_CreatedDate { get; set; }
        public DateTime? FPDC_UpdatedDate { get; set; }
        public long? FPDC_CreatedBy { get; set; }
        public long? FPDC_Updatedby { get; set; }
        public string AMST_FirstName { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public string FMBANK_BankName { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMCL_Id { get; set; }

    }
}
