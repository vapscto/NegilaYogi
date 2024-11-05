using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class EMAILSMSTemplateSettingDTO : CommonParamDTO
    {
        public long AMA_Id { get; set; }
        public string AMA_Activity { get; set; }
        public string AMA_Activity_Desc { get; set; }
        public string htmldata { get; set; }
        public Array modulellist { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string messageupdate { get; set; }

        public long MI_Id { get; set; }
        // public Array Activity_list { get; set; }

    }
}
