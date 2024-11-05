using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterMenuDTO : CommonParamDTO
    {
        public long IVRMMM_Id { get; set; }
        public string IVRMMM_MenuName { get; set; }
        public long IVRMM_Id { get; set; }
        public int IVRMMM_ParentId { get; set; }
        public bool IVRMMM_PageNonPageFlag { get; set; }
        public int IVRMMM_MenuOrder { get; set; }
    }
}
