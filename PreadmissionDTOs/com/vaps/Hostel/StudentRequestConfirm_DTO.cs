using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class StudentRequestConfirm_DTO
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

        public bool returnval { get; set; }
        public Array student_RequestList { get; set; }
        public Array student_RequestApp { get; set; }
        public Array room_list { get; set; }

    }
}
