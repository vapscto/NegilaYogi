using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentAttendanceEntryDTO
    {
        public long ASA_Id { get; set; }
        public long MI_Id { get; set; }
        public long? IVRMUL_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASA_Att_Type { get; set; }
        public string ASA_Att_EntryType { get; set; }
        public long ASMCL_Id { get; set; }
        public string asmcL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string asmC_SectionName { get; set; }
        public long? ISMP_Id { get; set; }
        public DateTime? ASA_Entry_DateTime { get; set; }
        public DateTime? ASA_FromDate { get; set; }
        public DateTime? ASA_ToDate { get; set; }
        public string ASA_ClassHeld { get; set; }
        public string ASA_Regular_Extra { get; set; }
        public string ASA_Network_IP { get; set; }
        public string ASA_Mac_Add { get; set; }
        public long? ASAS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ASA_AttendanceFlag { get; set; }
        public decimal ASA_Class_Attended { get; set; }   
        public long? ASASU_Id { get; set; }
        public long? PAMS_Id { get; set; }
        public string studentname { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? AMAY_RollNo { get; set; }       
        public Array academicYearList { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public Array monthList { get; set; }
        public Array subjectList { get; set; }
        public Array batchList { get; set; }
        public Array studentList { get; set; }
        public StudentAttTempDTO[] stdList { get; set; }
        public StudentAttTempDTO[] apsentStdList { get; set; }
        public bool returnval { get; set;}
        public string message { get; set; }
        public string monthflag { get; set; }
        public long? monthid { get; set; }        
        public decimal countclass { get; set; }
        public long countclass1 { get; set; }
        public string monthflag1 { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public string attendanceentryflag { set; get; }
        public long attdefaultdisplay { set; get; }
        public string attendanceflagtype { get; set; }        
        public string username { get; set; }
        public long Emp_Code { get; set; }
        public long ASALU_Id { get; set; }
        public long userId { get; set; }
        public string flag { get; set; }
        public long roleId { get; set; }
        public string rolename { get; set; }
        public Array CurrentYear { get; set; }
        public string classsecflag { get; set; }
        public string checkbatch { get; set; }
        public string checksubject { get; set; }
        public long ismS_Id { get; set; }
        public string ismS_SubjectName { get; set; }
        public Array periodlist { get; set; }
        public int TTMP_Id { get; set; }
        public int attcount { get; set; }
        public long asasB_Id { get; set; }
        public string amsT_RegistrationNo { get; set; }
        public Array getstandarad { get; set; }
        public TimeSpan? ASSC_PunchTime { get; set; }
        public Array admissionstandarad { get; set; }
        public string mobileprivileges { get; set; }
        public string stringmobileorportal { get; set; }
        public string Pagename { get; set; }
        public string Pageicon { get; set; }
        public string Pageurl { get; set; }
        public string att_entry_type { get; set; }
        public long? IVRMRMAP_Id { get; set; }
        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public Array ViewStudentPeriodWiseAttDetails { get; set; }
    }

    public class RFIDDATA
    {
        public string RFID { get; set; }
        public DateTime RFIDDatetime { get; set; }
        public string RFIDReaderIP { get; set; }
        public string RFIDAntenna { get; set; }
        public string RFIDstatus { get; set; }
    }
}
