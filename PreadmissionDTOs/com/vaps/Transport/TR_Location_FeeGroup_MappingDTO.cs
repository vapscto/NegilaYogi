using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TR_Location_FeeGroup_MappingDTO
    {
        public long TRLFM_Id { get; set; }
        public long TRML_Id { get; set; }
        public long FMG_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMAMI_IdY_Id { get; set; }
        public bool TRLFM_ActiveFlag { get; set; }
        public Array yearlist { get; set; }
        public Array locationlist { get; set; }
        public Array grouplist { get; set; }

        public string message { get; set; }

        public bool retrval { get; set; }

        public Array griddata { get; set; }
        public string FMG_GroupName { get; set; }
        public string TRML_LocationName { get; set; }
        public string ASMAY_Year { get; set; }
        public Array editdatadetails { get; set; }

        public string TRLFM_WayFlag { get; set; }

        public Array studentdata { get; set; }
    }

  
}
