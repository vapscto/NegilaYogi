using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class StaffRequestConfirm_DTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLHSTREQ_Id { get; set; }
        public long HLHSTREQC_Id { get; set; }

      
        public DateTime HLHSTREQC_RequestDate { get; set; }
        public long HRME_Id { get; set; }
        public long HLHSTREQC_HRMEId { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool HLHSTREQC_ACRoomFlg { get; set; }
        public bool HLHSTREQC_SingleRoomFlg { get; set; }
        public bool HLHSTREQC_VegMessFlg { get; set; }
        public bool HLHSTREQC_NonVegMessFlg { get; set; }
        public string HLHSTREQC_Remarks { get; set; }
        public string roletype { get; set; }
        public string HLHSTREQC_BookingStatus { get; set; }
        public bool HLHSTREQC_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public long? HRMRM_Id { get; set; }
        public Array staff_RequestList { get; set; }
        public Array staff_ApprovalList { get; set; }
        public Array room_list { get; set; }
    }
}
