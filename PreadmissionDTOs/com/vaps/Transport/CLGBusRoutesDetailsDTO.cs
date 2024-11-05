using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class CLGBusRoutesDetailsDTO
    {
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public long AMCO_Id { get; set; }
      
        public long ASMAY_Id { get; set; }
        public long AMB_Id { get; set; }
        public int AMB_Order { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public int ACMS_Order { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
      
        public string flag { get; set; }
        public string type { get; set; }
        public Array semesterlist { get; set; }
        public Array sectionlist { get; set; }
        public secid2[] secidlist { get; set; }
        public Array griddata { get; set; }
        public string amcO_CourseName { get; set; }
        public Array studentgriddata { get; set; }
    }
        public class secid2
        {
        public long AMSE_Id { get; set; }
        }
}
