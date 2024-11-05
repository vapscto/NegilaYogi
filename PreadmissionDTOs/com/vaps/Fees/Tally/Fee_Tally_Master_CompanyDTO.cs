using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.Tally
{
   public class Fee_Tally_Master_CompanyDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array Instititions { get; set; }
        public long Id { get; set; }
        public long FTMCOM_Id { get; set; }       
        public string FTMCOM_CompanyName { get; set; }
        public string FTMCOM_CompanyCode { get; set; }
        public bool FTMCOM_ActiveId { get; set; }
        public Array getarray { get; set; }
        public string return_val { get; set; }
       public string MI_Name { get; set; }
        public DateTime? FTMCOM_CreatedDate { get; set; }
    }
}
