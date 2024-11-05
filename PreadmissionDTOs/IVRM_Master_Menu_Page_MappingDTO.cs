using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class IVRM_Master_Menu_Page_MappingDTO : CommonParamDTO
    {
        public long IVRMMMPM_Id { get; set; }
        public long IVRMMM_Id { get; set; }
        public long IVRMP_Id { get; set; }

        public Array mastermenuarray { get; set; }
        public Array mastersubmenuarray { get; set; }
        public Array mastermodule { get; set; }

        public Array fillpages { get; set; }

        public Array fillgrid { get; set; }
        public Array fillprioussavedgrid { get; set; }

        public string menuname { get; set; }
        public string submenuname { get; set; }
        public string modulename { get; set; }
        public string pagename { get; set; }

        public long IVRMM_Id { get; set; }
        public IVRM_Master_Menu_Page_MappingDTO[] savetmpdata { get; set; }
        public IVRM_Master_Menu_Page_MappingDTO[] privilagedata { get; set; }
        public bool returnval { get; set; }

        public long IVRMMM_Idsubmenu { get; set; }

    }
}
