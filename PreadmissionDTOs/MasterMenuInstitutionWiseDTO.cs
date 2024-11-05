using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterMenuInstitutionWiseDTO : CommonParamDTO
    {
        public long IVRMMMI_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMMI_MenuName { get; set; }
        public long IVRMM_Id { get; set; }
        public long IVRMMMI_ParentId { get; set; }
        public bool IVRMMMI_PageNonPageFlag { get; set; }
        public int IVRMMMI_MenuOrder { get; set; }
    }
}
