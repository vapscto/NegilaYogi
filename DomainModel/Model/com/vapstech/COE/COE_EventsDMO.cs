using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.COE
{
    [Table("COE_Events", Schema = "COE")]
    public class COE_EventsDMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int COEE_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int COEME_Id { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public string COEE_EStartTime { get; set; }
        public string COEE_EEndTime { get; set; }

        public DateTime? COEE_EEndDate { get; set; }
        public string COEE_SMSMessage { get; set; }
        public bool COEE_SMSActiveFlag { get; set; }
        public string COEE_MailSubject { get; set; }

        // public string COEME_MailBody { get; set; }
        public string COEE_MailHeader { get; set; }
        public string COEE_MailFooter { get; set; }
        public string COEE_Mail_Message { get; set; }
        public string COEE_MailHTMLTemplate { get; set; }
        public bool COEE_MailActiveFlag { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }
        public bool COEE_AllDayFlag { get; set; }
        public bool COEE_RepeatFlag { get; set; }
        public string COEE_ReminderSchedule { get; set; }
        public string COEE_ReminderNotification { get; set; }
        public string COEE_Memos { get; set; }
        public bool COEE_StudentFlag { get; set; }
        public bool COEE_AlumniFlag { get; set; }
        public bool COEE_EmployeeFlag { get; set; }
        public bool COEE_ManagementFlag { get; set; }
        public bool COEE_ActiveFlag { get; set; }
        public bool COEE_HolidayFlag { get; set; }
        public string COEE_ReminderDur { get; set; }
        public string COEE_ReminderFlag { get; set; }
        public bool COEE_OtherFlag { get; set; }
        public List<COE_Events_ClassesDMO> coeEventClass { get; set; }
        public List<COE_Events_EmployeesDMO> coeEventEmployee { get; set; }
        public List<COE_Events_ImagesDMO> coeEventImage { get; set; }
        public List<COE_Events_OthersDMO> coeEventOther { get; set; }
        public List<COE_Events_VideosDMO> coeEventVideo { get; set; }
        public List<COE_Events_SMSMailPN_DMO> COE_Events_SMSMailPN_DMO { get; set; }



    }
}
