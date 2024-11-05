using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeTpinGenerationDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool returnval { get; set; }
        public Array getyearlist { get; set; }
        public Array gettpingeneratedliststudent { get; set; }
        public Array gettpinnotgeneratedliststudent { get; set; }
    }
}
