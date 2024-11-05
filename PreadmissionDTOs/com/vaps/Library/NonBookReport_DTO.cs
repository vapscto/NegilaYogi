using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class NonBookReport_DTO : CommonParamDTO
    {

        public long MI_Id { get; set; }
        public Array deptlist { get; set; }
        public Array lib_list { get; set; }
        public Array reportlist { get; set; }
        public string LMD_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long LMAL_Id { get; set; }
        public long ASMCL_Id { get; set; }

        public string Fromdate { get; set; }
        public string ToDate { get; set; }
        public string AGType { get; set; }
        public string TrnType { get; set; }
        public string IssueFromDate { get; set; }
        public string IssueToDate { get; set; }
        public string DueFromdate { get; set; }
        public string DueTodate { get; set; }
        public NonBookReport_DTO[] selectedClasslist {get;set;}
        public NonBookReport_DTO[] selectedSectionlist { get;set;}      
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public long ASMS_Id { get; set; }
        public long LMB_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string sectionName { get; set; }
        public string LMB_BookTitle { get; set; }

    }
}
