using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class LeaveDetailsCFDTO : CommonParamDTO
    {
        public long HRMLDCFM_Id { get; set; }
   
        public long HRMLDCF_Id { get; set; }
        public long MI_Id { get; set; }

    }
}
