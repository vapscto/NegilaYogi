using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
   public class VBadminDTO
    {

        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public Array getsateusers { get; set; }
        public Array getdistrictusers { get; set; }
        public Array getCOEEventDetails { get; set; }
        public Array getgraphsreport { get; set; }
    




        public string returnduplicatestatus { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }


    }
}
