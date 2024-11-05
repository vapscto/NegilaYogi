using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterInstitutionRoleandModuleMappingController : Controller
    {
        MasterInstitutionRoleandModuleMappingDelegate enqu = new MasterInstitutionRoleandModuleMappingDelegate();

        [HttpPost]
        public InstitutionRolePrivilegesDTO saveInstitution_Module_Pagedetail([FromBody] InstitutionRolePrivilegesDTO en)
        {

            return enqu.saveInstitution_Module_Pagedetails(en);
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public InstitutionRolePrivilegesDTO getdetail(int id)
        {
            InstitutionRolePrivilegesDTO data = new InstitutionRolePrivilegesDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.roleId = roleidd;


            return enqu.Institution_Module_PageDetails(data);
        }

        [Route("getModuleDropdownList/{id:int}")]
        public InstitutionRolePrivilegesDTO getModuleDropdownList(int id)
        {
            return enqu.getModuleDropdownList(id);
        }
        

         [HttpGet]
        [Route("getPagedetailsByModuleId/{id:int}")]
        public MasterRolePreviledgeDTO getPagedetails(int id)
        {
            return enqu.Module_PageDetails(id);
        }


       
        [Route("getPagedetailsByRoleType")]
        public MasterRolePreviledgeDTO getPagedetailsbyroletype([FromBody] MasterRolePreviledgeDTO data)
        {
            return enqu.Module_PageDetailsByRoleType(data);
        }

       
        [Route("deletedetails")]
        public InstitutionRolePrivilegesDTO Delete([FromBody] InstitutionRolePrivilegesDTO data)
        {
            return enqu.deleterec(data);
        }


    }
}
