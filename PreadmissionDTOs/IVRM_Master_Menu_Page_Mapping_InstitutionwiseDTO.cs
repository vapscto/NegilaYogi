using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO : CommonParamDTO
    {
        public long IVRMMMPMI_Id { get; set; }
        public long IVRMMMI_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public long MI_Id { get; set; }
        public Array mastermenuarray { get; set; }
        public Array mastersubmenuarray { get; set; }
        public Array mastermodule { get; set; }

        public string retrunMsg { get; set; }

        public Array fillpages { get; set; }

        public Array fillgrid { get; set; }
        public Array fillprioussavedgrid { get; set; }

        public Array GridDetails { get; set; }
        public Array fillinstitution { get; set; }

        public Array fillmodule { get; set; }

        public long IVRMIMP_Id { get; set; }

        public string menuname { get; set; }

        public int pageorder { get; set; }
        public string submenuname { get; set; }
        public string modulename { get; set; }
        public string pagename { get; set; }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO[] menuDTO { get; set; }
        public long IVRMM_Id { get; set; }
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO[] savetmpdata { get; set; }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO[] privilagedata { get; set; }
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO[] savetmpdata1 { get; set; }
        public string returnval { get; set; }
        public string MI_Name { get; set; }
        public long roleId { get; set; }
        public long Id { get; set; }
        public string Displayname { get; set; }


    }
}
