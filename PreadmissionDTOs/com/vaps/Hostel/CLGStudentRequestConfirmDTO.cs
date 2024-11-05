using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class CLGStudentRequestConfirmDTO
    {

        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLHSREQC_Id { get; set; }
        public long HLHSREQ_Id { get; set; }
        public long HLHSREQ_RequestDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool HLHSREQ_ACRoomFlg { get; set; }
        public bool HLHSREQ_SingleRoomFlg { get; set; }
        public bool HLHSREQ_VegMessFlg { get; set; }
        public bool HLHSREQ_NonVegMessFlg { get; set; }
        public string HLHSREQ_Remarks { get; set; }
        public string HLHSREQ_BookingStatus { get; set; }
        public string HLHSREQC_Remarks { get; set; }
        public long? HRMRM_Id { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public string ASMAY_Year { get; set; }
        public string HLMH_Name { get; set; }
        public long HLMF_Id { get; set; }
        public string HRMF_FloorName { get; set; }


        public bool returnval { get; set; }
        public Array student_RequestList { get; set; }
        public Array room_list { get; set; }
        public Array roomdetails { get; set; }
        public Array hostel_list { get; set; }
        public Array yearlist { get; set; }
        public Array floor_list { get; set; }
        public Array housewise_studentList { get; set; }
        public Array student_allotlist { get; set; }
        public bool bedcount { get; set; }

        public long HLHSREQCC_Id { get; set; }
        public DateTime? HLHSREQC_Date { get; set; }
        public long HRME_Id { get; set; }
        public bool? HLHSREQCC_ACRoomFlg { get; set; }
        public bool? HLHSREQCC_SingleRoomFlg { get; set; }
        public bool? HLHSREQCC_VegMessFlg { get; set; }
        public bool? HLHSREQCC_NonVegMessFlg { get; set; }
        public string HLHSREQCC_Remarks { get; set; }
        public string HLHSREQCC_BookingStatus { get; set; }
    }
}
