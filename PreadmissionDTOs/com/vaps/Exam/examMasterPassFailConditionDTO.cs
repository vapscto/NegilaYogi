using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class examMasterPassFailConditionDTO
    {
        public long EMPFC_Id { get; set; }
        public long MI_Id { get; set; }
        public string EXMC_CONDITION { get; set; }
        public bool Active_flag { get; set; }

    }
}
