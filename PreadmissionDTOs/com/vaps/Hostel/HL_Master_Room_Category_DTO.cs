using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class HL_Master_Room_Category_DTO
    {
        public long HLMRCA_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long? HLMH_Id { get; set; }
        public long? FMG_Id { get; set; }
        public string HLMRCA_RoomCategory { get; set; }
        public string HLMRCA_Description { get; set; }
        public long HLMRCA_MaxCapacity { get; set; }
        public bool HLMRCA_ActiveFlag { get; set; }
        public bool? HLMRCA_ACFlg { get; set; }
        public bool? HLMRCA_SharingFlg { get; set; }
        public decimal? HLMRCA_SORate { get; set; }
        public decimal? HLMRCA_RoomRate { get; set; }

        public bool returnval { get; set; }
        public bool duplicate { get; set; }

        public Array griddata { get; set; }
        public Array edit_Roomcatlist { get; set; }
        public Array hostellist { get; set; }
        public Array Feegroupe { get; set; }


    }
}
