using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AttendanceLPDTO : CommonParamDTO
    {
        // Login User
        public long ASALU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int IVRMUL_Id { get; set; }
        public string ASALU_Att_Exam_Flag { get; set; }
        public int ASALU_EntryTypeFlag { get; set; }
        // Login User

        // Login User Class
        public long ASALUC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        // Login User Class

        // Login User Class Subject
        public long ASALUCS_Id { get; set; }
        public long PAMS_Id { get; set; }
        // Login User Class Subject

        // Arrays
        public Array subjectList { get; set; }
        public Array classList { get; set; }
        public Array classsectionList { get; set; }
        public Array teacherList { get; set; }
        public Array accyear { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public string name { get; set; }
        public string classsection { get; set; }
        public ClassSectionDTO[] classsectionList1 { get; set; }
        public Array resultclasssectionData { get; set; }
        public MasterSubjectAllMDTO[] subjectsList { get; set; }
        public LoginPrevilegesDataDTO[] loginPData { get; set; }
        public LoginPrevilegesData_TempDTO[] selectedcls_sec_subs { get; set; }
        public string message { get; set; }
        public int count { get; set; }
        public bool returnval { get; set; }
        public AttendanceLPDTO[] duplicateList { get; set; }
        public AttendanceLPDTO[] CheckIsDuplicate { get; set; }
        public long HRME_Id { get; set; }
        public string operation { get; set; }
        // Arrays
    }
}
