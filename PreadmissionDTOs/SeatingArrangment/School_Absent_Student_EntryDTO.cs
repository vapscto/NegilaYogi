using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.SeatingArrangment
{
    public class School_Absent_Student_EntryDTO
    {
        public long MI_Id { get; set; }       
        public long UserId { get; set; }
        public long ESAABSTUSCH_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAROOM_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public DateTime? ESAABSTUSCH_ExamDate { get; set; }
        public bool ESAABSTUSCH_ActiveFlg { get; set; }
        public bool ReturnValue { get; set; }
        public Array GetYearList { get; set; }
        public Array GetClassList { get; set; }
        public Array GetSectionList { get; set; }
        public Array GetSubjectList { get; set; }
        public Array GetExamList { get; set; }
        public Array GetRoomList { get; set; }
        public Array GetSlotList { get; set; }
        public Array GetStudentList { get; set; }
        public Array GetSavedStudentList { get; set; }
        public string Studentname { get; set; }
        public string AMST_AdmNo { get; set; }
        public tempstudents[] tempstudents { get; set; }
        public Array GetAbsentReportList { get; set; }
    }

    public class tempstudents
    {
        public long AMST_Id { get; set; }
        public long ESAABSTUSCH_Id { get; set; }
    }
}
