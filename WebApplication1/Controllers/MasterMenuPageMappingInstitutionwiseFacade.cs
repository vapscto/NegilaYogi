using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterMenuPageMappingInstitutionwiseFacade : Controller
    {
        public MasterMenuPageMappingInstitutionwisInterface _MasterModule;

        public MasterMenuPageMappingInstitutionwiseFacade(MasterMenuPageMappingInstitutionwisInterface MasterpageModule)
        {
            _MasterModule = MasterpageModule;
        }

        [HttpPost]
        [Route("getalldetails")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO loaddata([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return _MasterModule.loaddata(data);
        }

        [Route("modulechange")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO modchange([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return _MasterModule.modchangedata(data);
        }

        [Route("mainmenuchange")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO mainmenuchan([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return _MasterModule.mainmenuchangedata(data);
        }

        [Route("deletemasterdata")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO deletemasterdata([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO id)
        {
            return _MasterModule.deletemasterdataa(id);
        }

        [Route("institutionchange/{id:int}")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO instituchan(int id)
        {
            return _MasterModule.institutionchan(id);
        }

        [HttpPost]
        [Route("submenuchange")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO submenuchan([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return _MasterModule.submenuchangedata(data);
        }

        [Route("Onchangedetails")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO orderchangedata([FromBody]IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO dto)
        {
            return _MasterModule.changeorderData(dto);
        }

        [Route("savemenudata")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO savedata([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return _MasterModule.savdata(data);
        }
    }
}
