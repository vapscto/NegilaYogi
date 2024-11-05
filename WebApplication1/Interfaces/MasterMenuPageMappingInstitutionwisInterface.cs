using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface MasterMenuPageMappingInstitutionwisInterface
    {
        IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO loaddata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data);
        IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO modchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data);
        IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO mainmenuchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data);
        IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO deletemasterdataa(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO ID);
        IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO institutionchan(int ID);
        
        IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO submenuchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data);

        IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO changeorderData(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data);
        IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO savdata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data);
    }
}
