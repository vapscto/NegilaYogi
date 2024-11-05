using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class WrittenTestScheduleWiseMarksDTO : CommonParamDTO
    {
        public long PASHWTM_Id { get; set; }
        public long PAWTS_Id { get; set; }
        public long PASWM_Id { get; set; }
    }
}
