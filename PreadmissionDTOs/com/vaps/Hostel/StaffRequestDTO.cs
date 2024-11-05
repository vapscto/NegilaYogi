using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class StaffRequestDTO
    {
        public long HLHSTREQ_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSTREQ_RequestDate { get; set; }
        public long HLMH_Id { get; set; }
        public long UserId { get; set; }
        public long HLMRCA_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool HLHSTREQ_ACRoomFlg { get; set; }
        public bool HLHSTREQ_SingleRoomFlg { get; set; }
        public bool HLHSTREQ_VegMessFlg { get; set; }
        public bool HLHSTREQ_NonVegMessFlg { get; set; }
        public string HLHSTREQ_Remarks { get; set; }
        public string HLHSTREQ_BookingStatus { get; set; }
        public bool HLHSTREQ_ActiveFlag { get; set; }
        public DateTime? HLHSTREQ_CreatedDate { get; set; }
        public DateTime? HLHSTREQ_UpdatedDate { get; set; }
        public long HLHSTREQ_CreatedBy { get; set; }
        public long HLHSTREQ_UpdatedBy { get; set; }
        public string msg { get; set; }
        public int ASMC_Order { get; set; }
        public bool ret { get; set; }
        public Array Editlist { get; set; }
        public bool duplicate { get; set; }
        public Array room_list { get; set; }
        public Array hostel_list { get; set; }
        public Array staff_wisedata { get; set; }
        public Array all_requestdata { get; set; }
        public bool HLHSTREQC_VegMessFlg { get; set; }
        public bool HLHSTREQC_SingleRoomFlg { get; set; }
        public bool HLHSTREQC_ACRoomFlg { get; set; }
        public string AMST_AdmNo { get; set; }

    }
}
