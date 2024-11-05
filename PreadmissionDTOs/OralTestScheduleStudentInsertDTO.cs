using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class OralTestScheduleStudentInsertDTO : CommonParamDTO
    {
        public long PAOTSS_Id { get; set; }
        public long PAOTS_Id { get; set; }
        public long PASR_Id { get; set; }
    }
}
