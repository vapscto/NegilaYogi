using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
  public class RackDetailsDTO : CommonParamDTO
    {
        public long LMRA_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMRA_RackName { get; set; }
        public string LMRA_Location { get; set; }
       // public long Rack_Floor_Id { get; set; }
        public bool LMRA_ActiveFlag { get; set; }
       // public Array floorlist { get; set; }
        public Array subjectlist { get; set; }
        public Array alldata { get; set; }
        public long LMRAS_Id { get; set; }
        public long LMS_Id { get; set; }
        public long Floor_Id { get; set; }
        public string LMRA_FloorName { get; set; }
        public bool LMRAS_ActiveFlg { get; set; }
        public string LMS_SubjectName { get; set; }
        public bool LMS_ActiveFlg { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string LMRA_DisplayColour { get; set; }
        public Array updatelist { get; set; }

        public string LMRA_BuildingName { get; set; }

    }
}
