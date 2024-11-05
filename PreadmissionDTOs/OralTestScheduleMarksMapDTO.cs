using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class OralTestScheduleMarksMapDTO : CommonParamDTO
    {
        public long PASHOM_Id { get; set; }
        public long PAOTM_Id { get; set; }
        public long PAOTS_Id { get; set; }
    }
}
