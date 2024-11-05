using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class MasterActivityDTO:CommonParamDTO
    {
        public long AMA_Id { get; set; }
        public string AMA_Activity { get; set; }
        public string AMA_Activity_Desc { get; set; }
        public Array MasterActivityname { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string messageupdate { get; set; }

        public long MI_Id { get; set; }
        // public Array Activity_list { get; set; }

    }
}
