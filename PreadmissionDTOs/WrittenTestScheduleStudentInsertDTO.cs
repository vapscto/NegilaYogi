using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class WrittenTestScheduleStudentInsertDTO : CommonParamDTO
    {
        public long PAWTSS_Id { get; set; }
        public long PAWTS_Id { get; set; }
        public long PASR_Id { get; set; }
    }
}
