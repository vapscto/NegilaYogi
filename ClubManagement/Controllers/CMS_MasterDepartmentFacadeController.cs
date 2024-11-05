using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubManagement.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.ClubManagement;

namespace ClubManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/CMS_MasterDepartmentFacade")]
    public class CMS_MasterDepartmentFacadeController : Controller
    {
        public CMS_MasterDepartmentInerface _cms;

        public CMS_MasterDepartmentFacadeController(CMS_MasterDepartmentInerface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]        
        public CMS_MasterDepartmentDTO loaddata(int id)
        {
            return _cms.loaddata(id);
           // return _cms.loaddata(id);
        }
        [HttpPost]
        [Route("savedata")]
        public CMS_MasterDepartmentDTO savedata([FromBody]CMS_MasterDepartmentDTO data)
        {
            return _cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_MasterDepartmentDTO deactive([FromBody]CMS_MasterDepartmentDTO data)
        {
            return _cms.deactive(data);
        }
        
        [HttpGet]
       
        [Route("loaddataconfigure/{id:int}")]
        public CMS_ConfigurationDTO loaddataconfigure(int id)
        {
            return _cms.loaddataconfigure(id);
           
        }
        [HttpPost]
        [Route("saveconfigure")]
        public CMS_ConfigurationDTO saveconfigure([FromBody]CMS_ConfigurationDTO data)
        {
            return _cms.saveconfigure(data);

        }
        [Route("confdeactive")]
        public CMS_ConfigurationDTO confdeactive([FromBody]CMS_ConfigurationDTO data)
        {
            return _cms.confdeactive(data);

        }
        //
    }
}