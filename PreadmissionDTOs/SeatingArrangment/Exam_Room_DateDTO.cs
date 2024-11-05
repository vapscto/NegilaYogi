using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.SeatingArrangment
{
    public class Exam_Room_DateDTO
    {
        public long ESAEDATE_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public long AMCO_Id { get; set; }
        public DateTime? ESAEDATE_ExamDate { get; set; }
        public bool? ESAEDATE_ActiveFlg { get; set; }
        public string Message { get; set; }
        public bool? Returnval { get; set; }
        public Array Getyearlist { get; set; }
        public Array Getexamlisst { get; set; }
        public Array Getuniversityexamlist { get; set; }
        public Array Getcourselist { get; set; }
        public Array Getslotlist { get; set; }
        public Array Getroomdetails { get; set; }
        public Array Getsavedroomdetails { get; set; }
        public Array GetSavedDetails { get; set; }
        public Array GetViewRoomDetails { get; set; }
        public Array GetEditedDateDetails { get; set; }
        public Array GetEditedRoomDetails { get; set; }
        public Room_Temp_Details[] Room_Temp_Details { get; set; }
        public string AMCO_CourseName { get; set; }
        public string EME_ExamName { get; set; }
        public string ESAUE_ExamName { get; set; }
        public string ESAESLOT_SlotName { get; set; }
        public string ASMAY_Year { get; set; }
        public int ASMAY_Order { get; set; }
        public long ESAEDATED_Id { get; set; }     
        public long ESAROOM_Id { get; set; }
        public long? ESAEDATED_AllotedCapacity { get; set; }
        public long? ESAROOM_RoomMaxCapacity { get; set; }
        public bool? ESAEDATED_ActiveFlg { get; set; }
        public string ESAROOM_RoomName { get; set; }

        /* Room Sitting Style Details */
        public long ESARSITSTY_Id { get; set; }
        public bool? ESARSITSTY_SameBranchSameSemFlg { get; set; }
        public bool? ESARSITSTY_DiffBranchSameSemFlg { get; set; }
        public bool? ESARSITSTY_SameBranchDiffSemFlg { get; set; }
        public bool? ESARSITSTY_DiffBranchDiffSemFlg { get; set; }
        public bool? ESARSITSTY_AnyBranchAnySemFlg { get; set; }
        public bool? ESARSITSTY_ActiveFlg { get; set; }
        public DateTime? ESARSITSTY_CreatedDate { get; set; }
        public DateTime? ESARSITSTY_UpdatedDate { get; set; }
        public long? ESARSITSTY_CreatedBy { get; set; }
        public long? ESARSITSTY_UpdatedBy { get; set; }        
        public Array GetRoomSittingStyleDetails { get; set; }
    }
    public class Room_Temp_Details
    {
        public long ESAEDATED_Id { get; set; }
        public long ESAROOM_Id { get; set; }
        public long? ESAEDATED_AllotedCapacity { get; set; }
        public bool? ESAEDATED_ActiveFlg { get; set; }
    }  
}
