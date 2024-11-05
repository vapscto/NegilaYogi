using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class statewisestudentadmissionDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }       
        public long IVRMMS_Id { get; set; }
        public long IMCC_Id { get; set; }
        public long IMC_Id { get; set; }
        public string flag { get; set; }
        public string ruralurban { get; set; }
        public string flagreligionrural { get; set; }
        public string categorycasteflag { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMR_Id { get; set; }
        public Array statestudentlist { get; set; }
        public Array acdlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semlist { get; set; }
        public Array seclist { get; set; }
        public Array check_list { get; set; }        
        public Array statelist { get; set; }
        public Array countrylist { get; set; }
        public Array religionlist { get; set; }
        public Array castecategory { get; set; }
        public Array mastercaste { get; set; }
        public string IMC_CasteName { get; set; }
        public string IVRM_CLM_coloumn { get; set; }        
        public string IVRM_CLG_PAR { get; set; }
        public string column_value { get; set; }
        public string AMST_AadharNo { get; set; }
        public string gender { get; set; }
        public string AMB_BranchName { get; set; }
        public int AMB_Order { get; set; }
        public Array quotalist { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime ? Todate { get; set; }
        public string months { get; set; }
        public Temp_Branchlist[] TempararyArrayListcoloumn { get; set; }
    }
    public class Temp_Branchlist
    {
        public long AMB_Id { get; set; }
        public string  AMB_BranchName { get; set; }
    }
}
