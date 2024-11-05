using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGTTSubjectWiseReportDTO
    {
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }


        public string AMCO_CourseName { get; set; }

        public long AMCO_Id { get; set; }

        public long AMB_Id { get; set; }
       
        public string AMB_BranchName { get; set; }
        public long AMSE_Id { get; set; }
     
        public string AMSE_SEMName { get; set; }



        public long ACMS_Id { get; set; }
       
        public string ACMS_SectionName { get; set; }

        public long ISMS_Id { get; set; }
        //public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ISMS_ActiveFlag { get; set; }


        public Array secdrp { get; set; }
        public Array year { get; set; }
        public Array clsdrp { get; set; }
        public Array subdrp { get; set; }
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }

        public CLGTTSubjectWiseReportDTO[] coursels { get; set; }

        public CLGTTSubjectWiseReportDTO[] branchls { get; set; }
        public CLGTTSubjectWiseReportDTO[] Time_Table { get; set; }
        public CLGTTSubjectWiseReportDTO[] classarray { get; set; }
        public CLGTTSubjectWiseReportDTO[] brharray { get; set; }
        public CLGTTSubjectWiseReportDTO[] subarray { get; set; }
        public CLGTTSubjectWiseReportDTO[] semarray { get; set; }
        public CLGTTSubjectWiseReportDTO[] secarray { get; set; }

        public CLGTTSubjectWiseReportDTO[] TT { get; set; }

        public Array getreportdata { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTFGD_Id { get; set; }
        public long TTFG_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMP_PeriodName { get; set; }
        public string staffName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array subjectlist { get; set; }
    }
}
