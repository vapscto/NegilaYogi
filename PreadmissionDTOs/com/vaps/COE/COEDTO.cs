using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.COE
{
    public class COEDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int COEME_Id { get; set; }
        public string COEME_EventName { get; set; }
        public int COEE_Id { get; set; }
        public List<COEDTO> EventsList { get; set; }
        public int eventCount { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public string COEE_SMSMessage { get; set; }
        public string COEE_Mail_Message { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }
        public bool COEE_SMSActiveFlag { get; set; }
        public bool COEE_MailActiveFlag { get; set; }
        public bool COEE_StudentFlag { get; set; }
        public bool COEE_EmployeeFlag { get; set; }
        public bool COEE_AlumniFlag { get; set; }
        public bool COEE_OtherFlag { get; set; }
        public List<COEDTO> EventsDetails { get; set; }
        public Array ClassList { get; set; }
        public Array GroupTypeList { get; set; } 
        public COEDTO[] Others_list { get; set; }
        public COEDTO[] selectedClasses { get; set; }
        public COEDTO[] selectedGroupTypeList { get; set; }
        public string COEEO_MobileNo { get; set; }
        public string COEEO_Emailid { get; set; }
        public string COEEO_Name { get; set; }
        public long asmcL_Id { get; set; }
        public string asmcL_ClassName { get; set; }
        public long hrmgT_Id { get; set; }
        public string hrmgT_EmployeeGroupType { get; set; }
        public bool SMS_Flag { get; set; }
        public bool Email_Flag { get; set; }
        public Array CoeEventsClassesList { get; set; }
        public Array CoeEventsEmployees { get; set; }
        public Array CoeEventsOthers { get; set; }
        public string msgStatus { get; set; }
        public string mailStatus { get; set; }
        public long IVRMGC_OTPMobileNo { get; set; }
        public Array configuration { get; set; }

    }
}
