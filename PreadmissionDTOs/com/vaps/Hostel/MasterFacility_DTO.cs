using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class MasterFacility_DTO
    {

        public long HLMFTY_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMFTY_FacilityName { get; set; }
        public string HLMFTY_FacilityDesc { get; set; }
        public string HLMFTY_FacilityFileName { get; set; }
        public string HLMFTY_FacilityFilePath { get; set; }
        public bool HLMFTY_ActiveFlag { get; set; }
        public long HLMFTY_CreatedBy { get; set; }
        public long HLMFTY_UpdatedBy { get; set; }
        public long UserId { get; set; }

        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public Array edit_facilitylist { get; set; }
        public Array get_facilitylist { get; set; }


    }
}
