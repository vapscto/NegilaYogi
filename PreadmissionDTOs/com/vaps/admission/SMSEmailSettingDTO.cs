using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class SMSEmailSettingDTO
    {
        public long ISES_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMIM_Id { get; set; }

        public string ISES_Template_Name { get; set; }
        public string ISES_SMSMessage { get; set; }
        public bool ISES_SMSActiveFlag { get; set; }
        public string ISES_MailSubject { get; set; }
        public string ISES_MailBody { get; set; }
        public string ISES_MailFooter { get; set; }
        public string ISES_Mail_Message { get; set; }
        public string ISES_MailHTMLTemplate { get; set; }
        public bool ISES_MailActiveFlag { get; set; }
        public long IVRMSTAUL_Id { get; set; }
       // public string ISES_Parameter { get; set; }
        //
        public string IVRMM_ModuleName { get; set; }
        public long IVRMIMP_Id { get; set; }
        public string IVRMMP_PageName { get; set; }
        public Array institutionModuleList { get; set; }
        public Array rolelist { get; set; }
        public Array institutionPageList { get; set; }
        public Array pageWiseHeaderList { get; set; }
        public Array pageWiseParameterList { get; set; }
        public Array emailSmsSettingList { get; set; }
        public Array emailtemplatelist { get; set; }
        public ICollection<IFormFile> File { get; set;}
        public SMS_MAIL_PARAMETER_DTO[] templateParameter { get; set; }
        public roleids[] roleids { get; set; }

        public int count { get; set; }
    }

    public class roleids
    {

        public long IVRMRT_Id { get; set; }
    }
}
