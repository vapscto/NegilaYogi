using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_SMS_Email_Setting")]
    public class SMSEmailSetting:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISES_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMIM_Id { get; set; }
        public string ISES_WhatsAppTemplateId { get; set; }
        public string ISES_Template_Name { get; set; }
        public string ISES_SMSMessage { get; set; }
        public bool ISES_SMSActiveFlag { get; set; }
        public string ISES_MailSubject { get; set; }
        public string ISES_MailBody { get; set; }
        public string ISES_MailFooter { get; set; }
        public string ISES_Mail_Message { get; set; }
        public string ISES_MailHTMLTemplate { get; set; }
        public bool ISES_MailActiveFlag { get; set; }
        public int? IVRMSTAUL_Id { get; set; }
       // public string ISES_Parameter { get; set; }
        public long? IVRMIMP_Id { get; set; }
        //public long? ISES_AlertBeforeDays { get; set; }
        public bool? ISES_EnableSMSCCFlg { get; set; }
        public string ISES_SMSCCMobileNo { get; set; }
        public bool ISES_EnableMailCCFlg { get; set; }
        public bool?  ISES_EnableMailBCCFlg { get; set; }
        //
        public string ISES_MailCCId { get; set; }
        public string ISES_MailBCCId { get; set; }
        public string ISES_IVRSTextMsg { get; set; }
        public string ISES_IVRSVoiceFile { get; set; }
        public string ISES_PNMessage { get; set; }
        public bool? ISES_PNActiveFlg { get; set; }
        public long? ISES_AlertBeforeDays { get; set; }

        public List<IVRM_EMAIL_ATT_DMO> IVRM_EMAIL_ATT_DMO { get; set; }
        public List<SMS_MAIL_SAVED_PARAMETER_DMO> SMS_MAIL_SAVED_PARAMETER_DMO { get; set; }
        public List<SMS_Email_Setting_RoleTypeDMO> SMS_Email_Setting_RoleTypeDMO { get; set; }
        public List<SMSEmailSettingUserMapping> SMSEmailSettingUserMapping { get; set; }

        public string ISES_TemplateId { get; set; }
    }
}
