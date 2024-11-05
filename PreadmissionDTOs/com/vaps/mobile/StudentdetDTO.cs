using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class StudentdetDTO
    {
        public class input
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
        }
        public long AMST_Id { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_Photoname { get; set; }
        public long AMST_MobileNo { get; set; }
        public DateTime AMST_Date { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
    }
}
