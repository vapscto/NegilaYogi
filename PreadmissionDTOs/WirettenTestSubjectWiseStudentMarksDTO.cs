using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class WirettenTestSubjectWiseStudentMarksDTO : CommonParamDTO
    {
        public long PASWMS_Id { get; set; }
        public long MI_Id { get; set; }
        public long PASWM_Id { get; set; }
        public long PASR_Id { get; set; }
        public decimal PASWMS_MarksScored { get; set; }
        public string PASWMS_PassFail { get; set; }

    }
}
