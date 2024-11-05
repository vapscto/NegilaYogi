using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
    public class PL_CI_Schedule_Company_JobTitle_CriteriaDTO
    {
        public Array EditDetails { get; set; }
        public long PLCISCHCOMJTCR_Id { get; set; }
        
        public long AMCO_Id { get; set; }
        public long PLMCLSMAP_Id { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public bool retval { get; set; }
        public Array pages { get; set; }
        public Array course { get; set; }
        public Array jobtitlelist {get;set;}
        public Array save { get; set; }     
        public decimal PLCISCHCOMJTCR_CutOfMark { get; set; }
        public bool PLCISCHCOMJTCR_ActiveFlag { get; set; }
        public string returnduplicatestatus { get; set; }
        public string AMCO_CourseName { get; set; }
        public string returnval { get; set; }
        public long User_Id { get; set; }
        public string PLCISCHCOMJTCR_OtherDetails { get; set; }
        public string PLCISCHCOMJT_JobTitle { get; set; }
        public string PLCISCHCOMJT_QulaificationCriteria { get; set; }     
        public string PLMCLSMAP_ClassName { get; set; }
        public long PLCISCHCOM_Id { get; set; }
    }
}
