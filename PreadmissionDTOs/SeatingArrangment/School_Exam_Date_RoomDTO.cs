using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.SeatingArrangment
{
    public class School_Exam_Date_RoomDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public long ESAEDATESCH_Id { get; set; }
        public long ESAEDATERSCH_Id { get; set; }
        public long ESAROOM_Id { get; set; }
        public long UserId { get; set; }
        public DateTime ESAEDATESCH_ExamDate { get; set; }
        public Array GetAcademicYearList { get; set; }
        public Array GetExamList { get; set; }
        public Array GetRoomList { get; set; }
        public Array GetExamSlotList { get; set; }
        public Array GetSavedDetails { get; set; }
        public Array GetaSavedRoomDetails { get; set; }
        public Array GetEditedDateDetails { get; set; }
        public Array GetEditedRoomDateDetails { get; set; }
        public Array GetViewRoomDetails { get; set; }
        public string ASMAY_Year { get; set; }
        public string EME_ExamName { get; set; }
        public string ESAESLOT_SlotName { get; set; }
        public string ESAROOM_RoomName { get; set; }
        public int ASMAY_Order { get; set; }
        public string ESAESLOT_StartTime { get; set; }
        public string ESAESLOT_EndTime { get; set; }
        public bool ESAEDATESCH_ActiveFlg { get; set; }
        public bool ESAEDATERSCH_ActiveFlg { get; set; }
        public DateTime? ESAEDATESCH_CreatedDate { get; set; }
        public bool ReturnValue { get; set; }
        public School_Room_Temp_Details[] School_Room_Temp_Details { get; set; }

        // Room Date Class Subject Mapping 
        public bool ESARMSCH_ActiveFlg { get; set; }
        public bool ESARMCSSCH_ActiveFlg { get; set; }
        public DateTime? ESARMSCH_ExamDate { get; set; }
        public DateTime? ESARMSCH_CreatedDate { get; set; }
        public DateTime? ExamDate { get; set; }
        public long ESARMSCH_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public long ESARMCSSCH_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int ASMCL_Order { get; set; }
        public Array GetRoomDetails { get; set; }
        public Array GetClassList { get; set; }
        public Array GetSubjectList { get; set; }
        public Array GetSavedClassSubjectList { get; set; }
        public Array GetViewRoomClassSubjectDetails { get; set; }
        public Array GetEditDetails { get; set; }
        public Array GetSeatAllotedReport { get; set; }
        public School_Room_ClassSubject_Temp_Details[] School_Room_ClassSubject_Temp_Details { get; set; }
    }
    public class School_Room_Temp_Details
    {
        public long ESAEDATERSCH_Id { get; set; }
        public long ESAEDATESCH_Id { get; set; }
        public long ESAROOM_Id { get; set; }
        public bool ESAEDATERSCH_ActiveFlg { get; set; }
    }
    public class School_Room_ClassSubject_Temp_Details
    {
        public long ESARMCSSCH_Id { get; set; }        
        public long ASMCL_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ESARMCSSCH_ActiveFlg { get; set; }
    }
}
