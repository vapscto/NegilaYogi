using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
   public class StudentFeeEnablePartialPaymentDTO
    {
        public long AMAY_RollNo { get; set; }
        public long MI_ID { get; set; }
        public string AMST_FirstName { get; set; }
        public long ASMCL_Id { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long ASMAY_Id { get; set; }
        public string FSEPP_Remarks { get; set; }
        public DateTime? FSEPP_RemarksDate { get; set; }
        public long FSEPP_Id { get; set; }
        public bool FSEPP_ActiveFlag { get; set; }
        public long AMST_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string retrunMsg { get; set; }
        public Array yearlist { get; set; }
        public Array alldata { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }
        public Array fillclass { get; set; }
        public Array fillsection { get; set; }
        public string ASMAY_Year { get; set; }

    }
}
