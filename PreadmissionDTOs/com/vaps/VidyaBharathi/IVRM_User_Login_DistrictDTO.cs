using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
   public class IVRM_User_Login_DistrictDTO
    {

        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long IVRMULDT_Id { get; set; }
        public long IVRMULST_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public long IVRMMD_Id { get; set; }
        public long IVRMULI_Id { get; set; }
        public long Id { get; set; }
        public String NormalizedUserName { get; set; }
        public bool IVRMULDT_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public string IVRMMS_Name { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public int savecount { get; set; }
        public int Duplicatecount { get; set; }
        public countrylistONE[] multiplecountryone { get; set; }
        public multiplstatelistONE[] multiplstatelistone { get; set; }
        public Array getcountry { get; set; }
        public Array statelist { get; set; }
        public Array getdistrict { get; set; }
        public Array getusers { get; set; }
        public Array getusermap { get; set; }


        public string returnduplicatestatus { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public class countrylistONE
        {
            public long ivrmmC_Id { get; set; }
        }
        public class multiplstatelistONE
        {
            public long IVRMMD_Id { get; set; }
        }

    }
}
