using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class IVRM_Mandatory_SettingDTO
    {
        public long IVRMMS_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public string IVRMMS_FieldName { get; set; }

        public string IVRMMS_Ngmodel { get; set; }

        public bool IVRMMS_MandatoryFlag { get; set; }
        public string retrunMsg { get; set; }

        public Array pagedropdown { get; set; }
        public Array pageList { get; set; }
        public IVRM_Mandatory_SettingDTO[] mandatoryfieldList { get; set; }
    }
}
