using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.ClubManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.ClubManagement;

namespace IVRMUX.Controllers.com.vapstech.ClubManagement
{
    [Produces("application/json")]
    [Route("api/CMS_MasterDepartment")]
    public class CMS_MasterDepartmentController : Controller
    {
        //CMS_MasterDepartment
        CMS_MasterDepartmentDelegate cms = new CMS_MasterDepartmentDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_MasterDepartmentDTO loaddata(int id)
        {
             id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //CMS_MasterDepartmentDTO data = new CMS_MasterDepartmentDTO();
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return cms.loaddata(id);
            
        }
        [HttpPost]
        [Route("savedata")]
        public CMS_MasterDepartmentDTO savedata([FromBody]CMS_MasterDepartmentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_MasterDepartmentDTO deactive([FromBody]CMS_MasterDepartmentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(data);
        }
        //configaration
        [HttpGet]
        [Route("loaddataconfigure/{id:int}")]
        public CMS_ConfigurationDTO loaddataconfigure(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //CMS_MasterDepartmentDTO data = new CMS_MasterDepartmentDTO();
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return cms.loaddataconfigure(id);

        }
        [HttpPost]
        [Route("saveconfigure")]
        public CMS_ConfigurationDTO saveconfigure([FromBody]CMS_ConfigurationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.saveconfigure(data);
        }
        [Route("confdeactive")]
        public CMS_ConfigurationDTO confdeactive([FromBody]CMS_ConfigurationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.confdeactive(data);
        }
        //saveconfigure
    }
}