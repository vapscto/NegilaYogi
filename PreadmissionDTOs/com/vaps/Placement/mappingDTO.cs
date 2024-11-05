using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
   public class mappingDTO
    {
        public string returnduplicatestatus;

        public long PLMCLSMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long AMCO_Id { get; set; }
        public string PLMCLSMAP_ClassName { get; set; }
        public string PLMCLSMAP_ClassFlg { get; set; }
        public string AMCO_CourseName { get; set; }
        public string PLMCLSMAP_Remarks { get; set; }
        public bool PLMCLSMAP_ActiveFlag { get; set; }
        public DateTime? PLMCLSMAP_CreatedDate { get; set; }
        public DateTime? PLMCLSMAP_UpdatedDate { get; set; }
        public long PLMCLSMAP_CreatedBy { get; set; }
        public long PLMCLSMAP_UpdatedBy { get; set; }
        public Array getsavedata { get; set; }
        public string returnval { get; set; }
        public Array save { get; set; }

        public Array editdata { get; set; }
    }
}
