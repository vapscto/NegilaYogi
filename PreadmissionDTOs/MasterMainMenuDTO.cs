using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterMainMenuDTO : CommonParamDTO
    {
        public long IVRMMM_Id { get; set; }
        public string IVRMMM_MenuName { get; set; }
        public long IVRMM_Id { get; set; }
        public int IVRMMM_ParentId { get; set; }
        public bool IVRMMM_PageNonPageFlag { get; set; }
        public int IVRMMM_MenuOrder { get; set; }
        public int IVRMMM_Id_select { get; set; }
        public string returnval { get; set; }
        public string modulename { get; set; }
        public string SubMenuName { get; set; }
        public Array masterModulesname { get; set; }
        public Array masterMainMenuName { get; set; }
        public Array GridDetails { get; set; }

        public Array studentDetails { get; set; }

        public string ivrmmM_Icon { get; set; }
        public string ivrmmM_Color { get; set; }

        public string dleleflag { get; set; }

        public int menuorder { get; set; }

        public string IVRMMMI_MenuName { get; set; }
    }
}
