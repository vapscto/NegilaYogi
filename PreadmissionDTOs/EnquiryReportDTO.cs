using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class EnquiryReportDTO
    {
        public string UserName { get; set; }
        public string PASE_emailid { get; set; }
        public long PASE_MobileNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public DateTime? PASE_Date { get; set; }
        public string PASE_EnquiryNo { get; set; }
        public string PASE_FirstName { get; set; }
        public string PASE_MiddleName { get; set; }
        public string PASE_LastName { get; set; }
        public string PASE_Address1 { get; set; }
        public string PASE_Address2 { get; set; }
        public string PASE_Address3 { get; set; }
        public int Id { get; set; }
        public string PASE_EnquiryDetails { get; set; }
    }
}
