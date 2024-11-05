using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.COE
{
    public class MasterCOEDTO
    {
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }

        //for master events table
        public int COEME_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public string COEME_SMSMessage { get; set; }
        public string COEME_MailSubject { get; set; }
        public string COEEO_Name { get; set; }
        public string COEEO_Emailid { get; set; }

        // public string COEME_MailBody { get; set; }
        public string COEME_MailHeader { get; set; }
        public string COEME_MailFooter { get; set; }
        public string COEME_Mail_Message { get; set; }
        public string COEME_MailHTMLTemplate { get; set; }
        public bool COEME_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        //for map events table
        public int COEE_Id { get; set; }
        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public string COEE_EStartTime { get; set; }
        public string COEE_EEndTime { get; set; }
        public string COEE_SMSMessage { get; set; }
        public bool COEE_SMSActiveFlag { get; set; }
        public string COEE_MailSubject { get; set; }
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
        //for coe classes
        public int COEEC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        //for coe employes
        public int COEEE_Id { get; set; }
        public long HRMGT_Id { get; set; }
        //for coe images
        public int COEEI_Id { get; set; }
        public string COEEI_Images { get; set; }

        public string intbfl_FilePath { get; set; }
        //for coe others
        public int COEEO_Id { get; set; }
        public long COEEO_MobileNo { get; set; }

        //for coe videos
        public int COEEV_Id { get; set; }
        public string COEEV_Videos { get; set; }
        public Array completeddate { get; set; }

        public Array parameterlist { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array master_eventlist { get; set; }
        public Array mapped_eventlist { get; set; }
        public Array eventlist_act { get; set; }
        public Array edit_m_event { get; set; }
        public Array edit_map_event { get; set; }
        public Array selected_master_event { get; set; }
        public Array stafftypelist { get; set; }

        //for editing
        public Array edit_stu_class_list { get; set; }
        public Array edit_emp_type_list { get; set; }
        public Array edit_oth_mobilenos_list { get; set; }
        public Array  edit_images_list { get; set; }
        public Array edit_videos_list { get; set; }

        //for storing
        public School_M_ClassDTO[] stu_class_list { get; set; }
        public HR_Master_DepartmentDTO[] emp_type_list { get; set; }
        public long[] oth_mobilenos_list { get; set; }
        public string[] images_list { get; set; }
        public string[] videos_list { get; set; }
        public ICollection<IFormFile> File { get; set; }
        public string ASMAY_Year { get; set; }


        public Array alumniClslist { get; set; }
        public Array stafflist { get; set; }
        public Array ttstafflist { get; set; }
        public Array sujectslistedit { get; set; }
        public string staffName { get; set; }
        // public string subjectName { get; set; }.
      //  public string staffNamelst { get; set; }
        public string yearName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
        public bool COEE_OtherFlag { get; set; }
        public COE_Events_OthersDTO[] others_list { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }

        //added By Akash
        public images_list[] images_listCoe { get; set; }
        public string name { get; set; }
    }
    public class COE_Events_OthersDTO
    {
        public long COEEO_MobileNo { get; set; }
        public string COEEO_Name { get; set; }
        public string COEEO_Emailid { get; set; }
    }
    public class images_list
    {
        public int COEEI_Id { get; set; }
        public int COEE_Id { get; set; }
        public string COEEI_Images { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
