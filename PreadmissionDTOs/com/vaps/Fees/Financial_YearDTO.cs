using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
  public class Financial_YearDTO
    {
      

        public long IMFY_Id { get; set; }
       
      
        public DateTime? IMFY_FromDate { get; set; }
        public DateTime? IMFY_ToDate { get; set; }
        public string IMFY_FinancialYear { get; set; }
        public string IMFY_AssessmentYear { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long IMFY_OrderBy { get; set; }
        public long year2 { get; set; }
    
        public int UserId { get; set; }

        public Array alldata1 { get; set; }
        public Array yeardata { get; set; }
        public Array rowcount { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public string ASMAY_Year { get; set; }
        public int MI_Id { get; set; }
        public long asmaY_Id { get; set; }
    }
}
