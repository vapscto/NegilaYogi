using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
   public class PL_Master_CompanyDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }

        public Array get_Placement { get; set; }

        public long PLMCOMP_Id { get; set; }      
        public string PLMCOMP_CompanyName { get; set; }
        public string PLMCOMP_CompanyAddress { get; set; }
        public string PLMCOMP_Website { get; set; }
        public string PLMCOMP_FacilityFilePath { get; set; }
        public bool PLMCOMP_ActiveFlag { get; set; }
        public DateTime? PLMCOMP_CreatedDate { get; set; }
        public DateTime? PLMCOMP_UpdatedDate { get; set; }
        public long PLMCOMP_CreatedBy { get; set; }
        public long PLMCOMP_UpdatedBy { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
    }
}
