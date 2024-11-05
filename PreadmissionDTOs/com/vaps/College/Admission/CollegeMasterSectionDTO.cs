using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeMasterSectionDTO
    {
        public long ACMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public string ACMS_SectionCode { get; set; }
        public int ACMS_Order { get; set; }
        public bool ACMS_ActiveFlag { get; set; }
        public int ACMS_MaxCapacity { get; set; }
        public Array getdetails { get; set; }
        public Array getdetails1 { get; set; }
        public Array editdetails { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Master_Section_CLg_Temp[] Master_Section_CLg_Temp { get; set; }
    }
    public class Master_Section_CLg_Temp
    {
        public long ACMS_Id { get; set; }
        public string ACMS_SectionName { get; set; }        
        public int ACMS_Order { get; set; }
    }
}
