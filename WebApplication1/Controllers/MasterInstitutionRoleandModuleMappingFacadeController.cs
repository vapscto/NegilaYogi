using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterInstitutionRoleandModuleMappingFacadeController : Controller
    {
        public MasterInstitutionRoleandModuleMappingInterface _enq;

        public MasterInstitutionRoleandModuleMappingFacadeController(MasterInstitutionRoleandModuleMappingInterface enqui)
        {
            _enq = enqui;
        }

        // POST api/values
        [HttpPost]
        public InstitutionRolePrivilegesDTO Post([FromBody] InstitutionRolePrivilegesDTO Trans)

        {
            return _enq.SaveInstitution_Module_Page(Trans);
        }

        [Route("getdetails")]
        public InstitutionRolePrivilegesDTO getdet([FromBody] InstitutionRolePrivilegesDTO Trans)
        {
            return _enq.GetDropdownList(Trans);
        }


        [Route("getPageDetailsBiModuleId/{id:int}")]
        public MasterRolePreviledgeDTO getPagedetailsByModuleId(int id)
        {
            return _enq.getPagedetailsByModuleId(id);
        }


        [Route("getModuleDropdownList/{id:int}")]
        public InstitutionRolePrivilegesDTO GetModuleDropdownList(int id)
        {
            return _enq.GetModuleDropdownList(id);
        }

        

        [Route("getPagedetailsByRoletype")]
        public MasterRolePreviledgeDTO getPagedetailsByRoletype([FromBody] MasterRolePreviledgeDTO data)
        {
            return _enq.getPagedetailsByRoleTypeId(data);
        }


        
      
        [Route("deletedetails")]
        public InstitutionRolePrivilegesDTO Deleterec([FromBody] InstitutionRolePrivilegesDTO data)
        {
            return _enq.deleterec(data);
        }
    }
}
