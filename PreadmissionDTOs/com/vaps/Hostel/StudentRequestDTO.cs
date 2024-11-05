using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class StudentRequestDTO
    {
        public long HLHSREQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime HLHSREQ_RequestDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool HLHSREQ_ACRoomFlg { get; set; }
        public bool HLHSREQ_SingleRoomFlg { get; set; }
        public bool HLHSREQ_VegMessFlg { get; set; }
        public bool HLHSREQ_NonVegMessFlg { get; set; }
        public string HLHSREQ_Remarks { get; set; }
        public string HLHSREQ_BookingStatus { get; set; }
        public bool HLHSREQ_ActiveFlag { get; set; }
        public DateTime HLHSREQ_CreatedDate { get; set; }
        public DateTime HLHSREQ_UpdatedDate { get; set; }
        public long HLHSREQ_CreatedBy { get; set; }
        public long HLHSREQ_UpdatedBy { get; set; }
        public string msg { get; set; }
        public string HLMH_Name { get; set; }
        public string HLMRCA_RoomCategory { get; set; }
        public string AMST_FirstName { get; set; }
        public long HLHSREQC_Id { get; set; }

        public Array hostel_list { get; set; }
        public Array room_list { get; set; }
        public bool duplicate { get; set; }
        public long UserId { get; set; }
        public Array Editlist { get; set; }
        public Array student_wisedata { get; set; }
        public Array all_requestdata { get; set; }
        public bool ret { get; set; }
        public string roleflag { get; set; }
        public long roleId { get; set; }
    }
}
