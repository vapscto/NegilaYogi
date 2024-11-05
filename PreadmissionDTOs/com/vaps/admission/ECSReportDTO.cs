using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class ECSReportDTO
    {
        public long MI_id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_id { get; set; }
        public Array getyearlist { get; set; }
        public Array getclasslist { get; set; }
        public Array getsectionlist { get; set; }
        public Array getreportdata { get; set; }
        public DateTime reportdate { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }

    }
}
