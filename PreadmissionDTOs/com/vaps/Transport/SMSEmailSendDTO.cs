using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class SMSEmailSendDTO
    {
        public Array YearList { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public SMSEmailSendDTO[] data_array { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array messagelist { get; set; }

        public string ISES_WhatsAppTemplateId { get; set; }
        public string ISES_NAME { get; set; }
        public long AMST_Id { get; set; }

        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string ASMAY_Year { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string studentname { get; set; }
        public long ASYST_Id { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool count { get; set; }
        public string success { get; set; }

        public string filterdata { get; set; }

        public long? AMST_FatherMobleNo { get; set; }
        public long? AMST_MotherMobileNo { get; set; }
        public string message { get; set; }
        public string neworall { get; set; }
        public string ALMST_FamilyPhoto { get; set; }
        
    }
}
