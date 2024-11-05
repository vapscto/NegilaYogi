using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.TT
{
   public class CLGTTCollegewiseConsolidatedReportDTO
    {
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public long ACMS_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array categorylist { get; set; }
        public long TTMC_Id { get; set; }
        public string TTMC_CategoryName { get; set; }
        public Array sectionlist { get; set; }
        public Array subject { get; set; }
        public Array day { get; set; }
        public Array getreportdata { get; set; }

    }
}
