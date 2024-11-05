using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
   public class StudentActiveInactiveReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public int ASMAY_Order { get; set; }

        public long AMCO_Id { get; set; }       
        public string AMCO_CourseName { get; set; }
        public bool AMCO_ActiveFlag { get; set; }
        public int AMCO_Order { get; set; }

        public long AMB_Id { get; set; }
        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }
        public string AMB_BranchName { get; set; }

        public long AMSE_Id { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public bool AMSE_ActiveFlg { get; set; }
        public string AMSE_SEMName { get; set; }

        public long ACMS_Id { get; set; }      
        public string ACMS_SectionName { get; set; }       
        public int ACMS_Order { get; set; }
        public bool ACMS_ActiveFlag { get; set; }

        public Array getreport { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array sectionlist { get; set; }
        public string AMCST_SOL { get; set; }
    }
}
