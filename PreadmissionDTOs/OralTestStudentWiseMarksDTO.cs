using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class OralTestStudentWiseMarksDTO : CommonParamDTO
    {
        public long PAOTMS_Id { get; set; }
        public long PASR_Id { get; set; }
        public long PAOTM_Id { get; set; }
        public decimal PAOTMS_Marks { get; set; }
    }
}
