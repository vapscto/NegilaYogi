using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface MasterMenuPageMappingInterface
    {
        IVRM_Master_Menu_Page_MappingDTO loaddata(int ID);
        IVRM_Master_Menu_Page_MappingDTO modchangedata(int ID);
        IVRM_Master_Menu_Page_MappingDTO mainmenuchangedata(int ID);
        IVRM_Master_Menu_Page_MappingDTO deletemasterdataa(int ID);
        IVRM_Master_Menu_Page_MappingDTO submenuchangedata(IVRM_Master_Menu_Page_MappingDTO data);
        IVRM_Master_Menu_Page_MappingDTO savdata(IVRM_Master_Menu_Page_MappingDTO data);


    }
}
