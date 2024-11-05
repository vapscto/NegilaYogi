using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class InstituteMainMenuDTO
    {
        public long IVRMMMI_Id { get; set; }

        public long MI_Id { get; set; }
        public string IVRMMMI_MenuName { get; set; }

        public long IVRMM_Id { get; set; }
        public long IVRMMMI_ParentId { get; set; }
        public bool IVRMMMI_PageNonPageFlag { get; set; }
        public int IVRMMMI_MenuOrder { get; set; }

        public string returnval { get; set; }
        public string modulename { get; set; }
        public string SubMenuName { get; set; }
        public Array masterModulesname { get; set; }
        public Array masterMainMenuName { get; set; }
        public Array masterSubMenuName { get; set; }
        public Array GridDetails { get; set; }

        public long IVRMMM_Id { get; set; }

        public List<MasterMainMenuDTO> SelectedMasterMenuDetails { get; set; }

        public Array fillinstitution { get; set; }

        public long roleId { get; set; }

        public string IVRMMMI_Icon { get; set; }
        public string IVRMMMI_Color { get; set; }
        public string MI_Name { get; set; }    
        
        public InstituteMainMenuDTO[] menuDTO { get; set; }

        public string retrunMsg { get; set; }

    }
}
