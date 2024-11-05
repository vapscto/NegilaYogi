using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class Room_Tariff_DTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRMRM_Id { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public long ASMAY_Id { get; set; }      
        public string ASMAY_Year { get; set; }
        public int ASMAY_Order { get; set; }
        public long HLMRTF_Id { get; set; }
        public long HRMRM_BedCapacity { get; set; }
        public decimal HLMRTF_SORate { get; set; }
        public decimal HLMRTF_RoomRate { get; set; }
        public bool HLMRTF_ActiveFlag { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array yeralist { get; set; }
        public Array room_list { get; set; }
        public Array gridlistdata { get; set; }
        public Array editlist { get; set; }

    }
}
