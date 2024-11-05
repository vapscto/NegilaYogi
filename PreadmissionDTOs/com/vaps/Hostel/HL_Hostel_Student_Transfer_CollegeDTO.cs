using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class HL_Hostel_Student_Transfer_CollegeDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLHSTRSC_Id { get; set; }
        public DateTime? HLHSALTC_TransferDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long ACMST_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public decimal HLHSTRSC_RoomFee { get; set; }
        public long HLHSTRSC_To_HLMRCA_Id { get; set; }
        public long HLMRCAC_To_HRMRM_Id { get; set; }
        public bool HLHSTRSC_EntireRoomReqdFlg { get; set; }
        public decimal HLHSTRSC_NewRoomFee { get; set; }
        public string HLHSTRSC_AllotRemarks { get; set; }
        public string HLHSTRSC_VacateRemarks { get; set; }
        public bool HLHSTRSC_ActiveFlag { get; set; }
        public string returnval { get; set; }
    }
}
