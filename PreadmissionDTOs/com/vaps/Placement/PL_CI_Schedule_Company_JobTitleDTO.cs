using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
    public class PL_CI_Schedule_Company_JobTitleDTO
    {
        public long?  MI_Id { get; set; }
        public long User_Id { get; set; }
        public long? PLMCLSMAP_Id { get; set; }
        public Array pages { get; set; }
        public Array Criterialist { get; set; }
        public string returnval { get; set; }
        public string PLCISCHCOMJT_QulaificationCriteria { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public string returnduplicatestatus { get; set; }
        public string PLCISCHCOMJT_JobTitle { get; set; }    
        public string page { get; set; }
        public bool? PLCISCHCOMJT_ActiveFlag { get;set; }
        public string PLCISCHCOMJT_OtherDetails { get; set; }
        public long? PLMCOMP_Id { get; set; }
        public string PLMCOMP_CompanyName { get; set; }
        public long PLCISCHCOM_Id { get; set; }
        public Array save { get; set; }
        public Array course { get; set; }
        public long PLCISCHCOMJT_NoOfInterviewRounds { get; set; }
        public Array EditDetails { get; set; }
        public bool retval { get; set; }    
    }
}
