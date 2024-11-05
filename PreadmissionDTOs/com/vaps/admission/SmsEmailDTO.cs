using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class SmsEmailDTO
    {
        public long? ISES_AlertBeforeDays { get; set; }
        public long ISES_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMIM_Id { get; set; }
        public long ISMHTML_Id { get; set; }
        public long IVRMHE_Id { get; set; }
        public string ISES_IVRSTextMsg { get; set; }
        public string ISES_IVRSVoiceFile { get; set; }
        public string ISES_PNMessage { get; set; }
        public bool? ISES_PNActiveFlg { get; set; }
        public string ISES_Template_Name { get; set; }
        public string ISES_SMSMessage { get; set; }
        public bool ISES_SMSActiveFlag { get; set; }
        public bool ISES_EnableSMSCCFlg { get; set; }
        public bool ISES_EnableMailCCFlg { get; set; }
        public bool ISES_EnableMailBCCFlg { get; set; }
        public string ISES_MailSubject { get; set; }
        public string ISES_MailBody { get; set; }
        public string ISES_MailFooter { get; set; }
        public string ISES_Mail_Message { get; set; }
        public string ISES_MailHTMLTemplate { get; set; }
        public bool ISES_MailActiveFlag { get; set; }
        public int? IVRMSTAUL_Id { get; set; }
        public long? IVRMIMP_Id { get; set; }
        public Array mobilenolist { get; set; }
        public Array emailistbcc { get; set; }
        public Array emailistmcc { get; set; }
        public Array rolelist { get; set; }
        public Array emailSmsSettingList { get; set; }
       // public Array pageWiseHeaderList { get; set; }
        

        public Array parameterList { get; set; }
        public Array editfiles { get; set; }

        public long ISMP_ID { get; set; }
        public string ISMP_NAME { get; set; }
      
        public string message { get; set; }
        public Array pageWiseHeaderList { get; set; }
        public Array institutionPageList { get; set; }
        public Array emailtemplatelist { get; set; }
        public SMS_MAIL_PARAMETER_DTO[] templateParameter { get; set; }
        public bool returnval { get; set; }


        public emailfiledto[] filelist { get; set; }
        public Mobile_Number_DTO[] mobile_list_dto { get; set; }
        public Email_Id_DTO[] email_list_dtocc { get; set; }

        public Email_Id_DTO[] email_list_dto { get; set; }
        public roleids[] roleids { get; set; }

        public string ISES_TemplateId { get; set; }
    }


    public class emailfiledto
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string status { get; set; }
    }

}
