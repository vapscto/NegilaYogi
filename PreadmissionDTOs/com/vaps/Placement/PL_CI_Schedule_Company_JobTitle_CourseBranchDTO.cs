using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
   public class PL_CI_Schedule_Company_JobTitle_CourseBranchDTO
    {
        public Array pages { get; set; }
        public Array course { get; set; }
        public Array branch { get; set; }
        public Array save { get; set; }
        public long PLCISCHCOMJTCB_Id { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long User_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool PLCISCHCOMJTCB_ActiveFlag { get; set; }
        public string PLCISCHCOMJTCB_ApplicableSEM { get; set; }
        public string returnduplicatestatus { get; set; }
        public string retval { get; set; }
        public string returnval { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string PLCISCHCOMJT_JobTitle { get; set; }
        public DateTime PLCISCHCOMJTCB_CreatedDate { get; set; }
        public DateTime PLCISCHCOMJTCB_UpdatedDate { get; set; }
        public long PLCISCHCOMJTCB_CreatedBy { get; set; }
        public long PLCISCHCOMJTCB_UpdatedBy { get; set; }
        public Array EditDetails { get; set; }
    }
}
