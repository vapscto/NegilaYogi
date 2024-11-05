using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Scholorship
{
   public class ScholorshipMasterDTO
    {
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long userId { get; set; }
        public Array Country { get; set; }
        public long IVRMMC_Id { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public string IVRMMC_CountryCode { get; set; }
        public int IVRMMC_MobileNoLength { get; set; }
        public int IVRMMC_Default { get; set; }
        public string IVRMMC_Currency { get; set; }
        public string IVRMMC_CountryPhCode { get; set; }
        public string IVRMMC_Nationality { get; set; }
        public bool? IVRMMC_ActiveFlag { get; set; }
        public Array CountryDetails { get; set; }
        public string returnval { get; set; }
        public string IVRMMS_Name { get; set; }
        public Array statelist { get; set; }
        public Array distictlist { get; set; }
        public string IVRMMS_Code { get; set; }
        public long? IVRMMS_Id { get; set; }
        public bool? IVRMMS_ActiveFlag { get; set; }
        public Array talukalist { get; set; }
        public Array Panchayatlist { get; set; }
        public Array getreport { get; set; }
        public Array jillaDetails { get; set; }
        public Array talukdetails { get; set; }
        public bool? IVRMMS_AllowScholashipFlg { get; set; }
        public long ASMCL_Id { get; set; }
        public decimal ? IVRMMS_MaxScholarshipQuota { get; set; }
        public decimal? SMMAY_MaxSxholarship { get; set; }
        public Array roleflagArray { get; set; }
        public string AMST_ScholarshipStatus { get; set; }
        public Array pranthlist { get; set; }
        public Array Master_VibhagList { get; set; }
        public Array documentlist { get; set; }

        public long AMST_Id { get; set; }
    }
}
