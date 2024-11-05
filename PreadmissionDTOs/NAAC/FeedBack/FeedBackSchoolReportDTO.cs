using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.FeedBack
{
   public  class FeedBackSchoolReportDTO
    {
        public long MI_Id { get; set; }
        public Array getyear { get; set; }
        public Array class_list{ get; set; }
        public Array feedbacktype { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMTY_Id { get; set; }
        public Array getReport { get; set; }
        public Array getcount { get; set; }
        public string optionflag { get; set; }       
        public long ASMCL_Id { get; set; }
        public bool type { get; set; }
        public long ASMS_Id { get; set; }
        public Array classlist { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_Order { get; set; }
        public Array sectionlist { get; set; }
        public string ASMC_SectionName { get; set; }
        public int ASMC_Order { get; set; }
    }
}
