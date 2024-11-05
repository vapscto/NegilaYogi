using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class YearEndReportDTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array academicYear { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public Array yearEndReport { get; set; }
        public string yearName { get; set; }
        public string admNo { get; set; }
        public string studentName { get; set; }
        public string className { get; set; }
        public string sectionName { get; set; }
        public string houseName { get; set; }
        public string divisionName { get; set; }
        public double points { get; set; }
        public int count { get; set; }
        public string radioVal { get; set; }
        public long totalNo { get; set; }
        public string SPCCME_EventName { get; set; }
        public string SPCCMEV_EventVenue { get; set; }
        public DateTime? SPCCE_StartDate { get; set; }
        public string SPCCESTR_Rank { get; set; }
        public string ASMAY_Year { get; set; }
        public string Type { get; set; }
        public string SPCCMSCC_SportsCCName { get; set; }

        public YearEndReportDTO[] selectedClasslist { get; set; }
        public YearEndReportDTO[] selectedSectionlist { get; set; }

    }
}
