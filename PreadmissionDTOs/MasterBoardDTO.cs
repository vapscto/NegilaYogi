using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterBoardDTO : CommonParamDTO
    {
        public long IVRMMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMB_Name { get; set; }
        public string IVRMMB_Description { get; set; }
        public bool Is_Active { get; set; }
        public string returnval { get; set; }

        public Array boardList { get; set; }
    }
}
