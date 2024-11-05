using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
   public class DocumentViewClgDTO
    {
        public long asmay_id { get; set; }
        public long user_id { get; set; }
        public long mi_id { get; set; }
        public Array fillyear { get; set; }
        public Array registrationList { get; set; }

        public DocumentViewDTO[] registrationListnew { get; set; }
        public Array admissioncatdrp { get; set; }

        public Array admissioncatdrpall { get; set; }
        public Array prospectusPaymentlist { get; set; }
        public int payementcheck { get; set; }
        public int configurationsettings { get; set; }
        public Array doc_list { get; set; }
        public long PASRD_Id { get; set; }
        public string Document_Path { get; set; }
        public string AMSMD_DocumentName { get; set; }

        public long paca_id { get; set; }

        public string PACA_FirstName { get; set; }
        public string PACA_MiddleName { get; set; }
        public string PACA_LastName { get; set; }
        public string PACA_RegistrationNo { get; set; }
        public string PACA_Student_Pic_Path { get; set; }

        public string AMCO_CourseName { get; set; }

        public DocumentViewDTO[] ddoc { get; set; }

        public long AMSMD_Id { get; set; }

        public Array studentDetailsTEmp { get; set; }

        public Array studentDetails { get; set; }
        public Array remarkschedulelist { get; set; }
        

        public Array studentDetailsHelth { get; set; }

        public long AMCO_Id { get; set; }

        public Array allcourse { get; set; }

        public long HRME_Id { get; set; }

        public string HRME_EmployeeFirstName { get; set; }

        public Array stafflist { get; set; }
        public Array OralTestScheduleclg { get; set; }

        public Array vcOralTestScheduleClg { get; set; }

        public Array OralTestScheduleClgcount { get; set; }

        public Array mstConfig { get; set; }
        public Array overallOralTestSchedulecountClg { get; set; }

        public Array SelectedStudentDetails { get; set; }
        public Array schedulelist { get; set; }
        

        public long IVRMSTAUL_Id { get; set; }


        public long PAOTSC_Id { get; set; }
     
        public long ASMAY_Id { get; set; }

        public DateTime PAOTSC_Date { get; set; }

        public string PAOTSC_ScheduleName { get; set; }
        public DateTime? PAOTSC_ScheduleDate { get; set; }
        public string PAOTSC_ScheduleFromTime { get; set; }
        public string PAOTSC_ScheduleToTime { get; set; }
        public string PAOTSC_To_AM_PM { get; set; }
        public string PAOTSC_LB_FT { get; set; }

        public string PAOTSC_LB_TT { get; set; }

        public string PAOTSC_TimePeriod { get; set; }

        public string PAOTSC_TPFlag { get; set; }

        public long PAOTSC_Strength { get; set; }

        public string PAOTSC_Remarks { get; set; }

        public string returnvalue { get; set; }
             public bool autoschedule { get; set; }
        public DateTime PlannedDate { get; set; }

        public string PlannedStartTime { get; set; }
        public string PlannedEndTime { get; set; }
        public string MeetingTopic { get; set; }
        public string meetingid { get; set; }
        public long LMSLMEET_Id { get; set; }
        public bool returnval { get; set; }
        
        public List<StudentDetailsClgDTO> SelectedStudentData { get; set; }

        public List<StudentDetailsClgDTO> SelectedStudentDataForEdit { get; set; }

    }
}
