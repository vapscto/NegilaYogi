using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class MasterGuestDTO:CommonParamDTO
    {
        public long Guest_Id { get; set; }
        public long MI_Id { get; set; }
        public string Guest_Name { get; set; }
        public string Guest_address { get; set; }
        public string Guest_Email_Id { get; set; }
        public string Guest_Phone_No { get; set; }
        public bool Guest_ActiveFlag { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array pulishlist { get; set; }
    }
}
