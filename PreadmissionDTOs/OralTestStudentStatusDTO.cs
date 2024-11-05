using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class OralTestStudentStatusDTO : CommonParamDTO
    {
        public long PAOTSS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long PASR_Id { get; set; }
        public decimal PAOTSS_OverallMarks { get; set; }
        public string PAOTSS_Status { get; set; }
    }
}
