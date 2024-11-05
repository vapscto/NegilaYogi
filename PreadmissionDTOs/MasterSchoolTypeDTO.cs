using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterSchoolTypeDTO : CommonParamDTO
    {
        public long IVRMMTYP_Id { get; set; }
        public string IVRMMTYP_Type { get; set; }
        public string IVRMMTYP_Description { get; set; }
        public bool Is_Active { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public Array schoolTypeList { get; set; }
    }
}
