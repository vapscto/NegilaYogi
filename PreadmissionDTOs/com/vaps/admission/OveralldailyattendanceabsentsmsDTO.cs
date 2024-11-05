using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class OveralldailyattendanceabsentsmsDTO
    {
        public long ASMAY_Id { get; set; }
        public long? Emp_Code { get; set; }
        public long? userId { get; set; }
        public long? roleId { get; set; }
        public long ASMCL_Id { get; set; }
        public long miid { get; set; }
        public long AMST_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public DateTime? fromdate { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string ASMCL_className { get; set; }
        public string studentname { get; set; }
        public string username { get; set; }
        public string rolename { get; set; }
        public string classsection { get; set; }
        public string asmC_SectionName { get; set; }
        public string flag { get; set; }
        public string message { get; set; }
        public string ASA_Network_IP { get; set; }
        public bool returnval { get; set; }
        public Array academicListdefault { get; set; }
        public Array student_teacherList { get; set; }
        public Array studentAttendanceList { get; set; }
        public Array academicList { get; set; }
        public long? AMST_FatherMobleNo { get; set; }
        public long? AMST_MotherMobileNo { get; set; }
        public string attType { get; set; }
        public string ASA_Dailytwice_Flag { get; set; }
        public string ASA_AttendanceFlag { get; set; }
        public OveralldailyattendanceabsentsmsDTO[] absentlist { get; set; }

        public bool categoryflag { get; set; }
        public string logo_path { get; set; }
        public Array category_list { get; set; }
        public OveralldailyattendanceabsentsmsDTO[] att_classes_list { get; set; }

        public OveralldailyattendanceabsentsmsDTO[] att_section_list { get; set; }

        public long AMC_Id { get; set; }
        public Array AMC_logo { get; set; }
        public long? ASA_CommunicationSentFlg { get; set; }

        public bool? FirstHalf { get; set; }
        public bool? SecondHalf { get; set; }
    }

  
}
