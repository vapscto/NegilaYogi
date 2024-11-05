using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class PromotionSmsAndEmailDetailsReport_DTO
    {
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public Array allacademicyear { get; set; }
        public Array allclasslist { get; set; }
        public Array allsectionlist { get; set; }
        public Array studentdetails { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string studentname { get; set; }
        public string AMST_FirstName { get; set; }
        public string EPRD_Remarks { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string EPRD_ClassPromoted { get; set; }
        public string EPRD_Promotionflag { get; set; }
        public string EPRD_PromotionName { get; set; }
        public string statusremarks { get; set; }
        public string message { get; set; }
        public bool sms { get; set; }
        public bool email { get; set; }

        public PromotionSmsAndEmailDetailsReport_DTO[] finalstudentlist { get; set; }

    }
}
