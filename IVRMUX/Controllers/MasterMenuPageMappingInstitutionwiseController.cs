using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterMenuPageMappingInstitutionwiseController : Controller
    {
        MasterMenuPageMappingInstitutionwiseDelegate od = new MasterMenuPageMappingInstitutionwiseDelegate();
        [HttpGet]
        [Route("Getdetails/{id:int}")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO loaddata(int id)
        {
            IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data = new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Id = UserId;

            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.roleId = roleidd;

            return od.loaddata(data);
        }

        [Route("institutionchange/{id:int}")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO instituchan(int id)
        {
            return od.institutionchan(id);
        }

        [Route("modulechange")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO modchange([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return od.modchangedata(data);
        }

        [Route("mainmenuchange")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO mainmenuchan([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return od.mainmenuchangedata(data);
        }

        [Route("deletemasterdata")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO deletemasterdata([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO id)
        {
            return od.deletemasterdataa(id);
        }

        [HttpPost]
        [Route("submenuchange")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO submenuchan([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return od.submenuchangedata(data);
        }

        [Route("validateordernumber")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO validateordernumber([FromBody]IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO dto)
        {
            // HR_Master_GroupTypeDTO dto = new HR_Master_GroupTypeDTO();
            // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.Onchangedetails(dto);
        }

        [Route("savemenudata")]
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO savedata([FromBody] IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            return od.savdata(data);
        }
    }
}
