using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
   public class ChangeOfBranchReportDTO
    {
        public long MI_Id { get; set; }
        public Array academiclist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array alldata { get; set; }
        public Array reportdata { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMCO_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }

    }
}
