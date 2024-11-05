using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class AdmissionStatusDTO : CommonParamDTO
    {
        public long PAMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string PAMST_Status { get; set; }
        public string PAMST_StatusFlag { get; set; }

        public Array academicstatuslist { get; set; }

        public bool returnval { get; set; }

        public string returnduplicatestatus { get; set; }

        public int active { get; set; }
    }
}
