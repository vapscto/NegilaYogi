using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
   public class PL_CI_Schedule_CompanyDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public Array get_Company { get; set; }
        public Array get_details { get; set; }
        public Array get_Schdule { get; set; }
        public Array editdata { get; set; }



        public long PLCISCHCOM_Id { get; set; }
        public long PLCISCH_Id { get; set; }
        public long PLMCOMP_Id { get; set; }
        public DateTime? PLCISCHCOM_DriveFromDate { get; set; }
        public DateTime? PLCISCHCOM_DriveToDate { get; set; }
        public string PLCISCHCOM_JobDetails { get; set; }
        public bool PLCISCHCOM_ActiveFlag { get; set; }
        public DateTime? PLCISCHCOM_CreatedDate { get; set; }
        public long PLCISCHCOM_CreatedBy { get; set; }
        public DateTime? PLCISCHCOM_UpdatedDate { get; set; }
        public long PLCISCHCOM_UpdatedBy { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string PLMCOMP_CompanyName { get; set; }
        public string PLCISCH_JobDetails { get; set; }
    }
}
