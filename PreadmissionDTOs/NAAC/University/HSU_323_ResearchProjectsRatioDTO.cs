using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class HSU_323_ResearchProjectsRatioDTO
    {
        public long NC323RPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long NC323RPR_Year { get; set; }
        public string NC323RPR_ProjName { get; set; }
        public string NC323RPR_PricipalName { get; set; }
        public string NC323RPR_AgencyName { get; set; }
        public string NC323RPR_Type { get; set; }
        public string NC323RPR_DeptName { get; set; }
        public decimal NC323RPR_FundProvided { get; set; }
        public string NC323RPR_ProjDuration { get; set; }
        public bool NC323RPR_ActiveFlag { get; set; }
        public long NC323RPR_CreatedBy { get; set; }
        public long NC323RPR_UpdatedBy { get; set; }
        public DateTime? NC323RPR_CreatedDate { get; set; }
        public DateTime? NC323RPR_UpdatedDate { get; set; }
        

        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long asmaY_Id { get; set; }
        public long hrmD_Id { get; set; }
        public bool duplicate { get; set; }
        public HSU_323_ResearchProjectsRatioDTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array departmentlist { get; set; }
        public long NC323RPRF_Id { get; set; }
        public string Flag { get; set; }

        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public HSU_323_ResearchProjectsRatioDTO[] selected_Inst { get; set; }
        public long cycleid { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
        public Array yearlist { get; set; }
        public Array yearlist1 { get; set; }
        public Array govtsclist { get; set; }
        public Array govtsclistfiles { get; set; }


    }
}
