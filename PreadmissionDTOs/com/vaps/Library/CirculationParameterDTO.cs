using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class CirculationParameterDTO:CommonParamDTO
    {
        public long Circ_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMC_Id { get; set; }
        public string CM_Id { get; set; }
        public long Max_Issue_Items { get; set; }
        public long Max_Issue_Days { get; set; }
        public long Max_No_Renewals { get; set; }
        public long LBCPAS_NoOfItems { get; set; }
        public long LBCPAS_IssueDays { get; set; }
        public long LBCPAS_NoOfRenewals { get; set; }
        public string Circ_Flag { get; set; }
        public bool Circ_ActiveFlag { get; set; }
        public string LMC_CategoryName { get; set; }
        public Array categorylist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array alldata { get; set; }
        public Array fillclass { get; set; }
        public long LBCPA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public Array fillemp { get; set; }
        public long HRMGT_Id { get; set; }
        public long LNBCPA_Id { get; set; }
        public Array issuetype { get; set; }
        public string LBCPA_Flg { get; set; }
        public string Catgname { get; set; }
        public string BOOKFLAG { get; set; }
        public string issuertype1 { get; set; }
        public string MI_SchoolCollegeFlag { get; set; }
        public bool LBCPAS_ActiveFlg { get; set; }
        public string LBCPA_IssueRefFlg { get; set; }
        public long Parmeter_Id { get; set; }

        public bool? LBCPA_ExcludeHolidayFlg { get; set; }
    }
}
