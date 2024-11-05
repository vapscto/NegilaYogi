using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class WrittenTestSutdentTotalMarksDTO : CommonParamDTO
    {
        public long PAWMS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long PASR_Id { get; set; }
        public decimal PASR_TotalMarksScored { get; set; }
        public string PASR_Status { get; set; }
    }
}
