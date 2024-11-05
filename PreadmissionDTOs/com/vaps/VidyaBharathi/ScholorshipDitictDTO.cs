using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Scholorship
{
   public class ScholorshipDitictDTO
    {
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long userId { get; set; }
        public long IVRMMD_Id { get; set; }
        public string IVRMMD_Name { get; set; }
        public string IVRMMD_Code { get; set; }
        public bool? IVRMMD_ActiveFlag { get; set; }
        public long IVRMMS_Id { get; set; }
        public string returnval { get; set; }
        public Array statedetails { get; set; }
        public Array distictsetails { get; set; }
        public long IVRMMC_Id { get; set; }
        public string IVRMMS_Name { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public Array disctictlist { get; set; }
        public Array talukalist { get; set; }
        public bool? IVRMMT_ActiveFlag { get; set; }
        public long IVRMMT_Id { get; set; }
        public string IVRMMT_Name { get; set; }
        public string IVRMMPCT_Name { get; set; }
        public long IVRMMPCT_Id { get; set; }
        public bool? IVRMMPCT_AllowScholashipFlg { get; set; }
        public bool IVRMMPCT_ActiveFlag { get; set; }
        public bool? IVRMMDAY_AllowScholashipFlg { get; set; }
        public bool? IVRMMT_AllowScholashipFlg { get; set; }
        public long? IVRMMDAY_MaxScholarshipQuota { get; set; }
        public long? IVRMMT_MaxScholarshipQuota { get; set; }
        public long? IVRMMPCT_MaxScholarshipQuota { get; set; }
        public long? IVRMMS_MaxScholarshipQuota { get; set; }
        public Array Panchayatlist { get; set; }
        public long IVRMMDAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string IVRMMSCH_SchoolName { get; set; }
        public bool? IVRMMDAY_MaxQuotaCheckingApplFlg { get; set; }
    }
}
