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
    [Route("api/CMS_Master_Installments")]
    public class CMS_Master_InstallmentsController : Controller
    {
        CMS_Master_InstallmentsDelegate cms = new CMS_Master_InstallmentsDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_Master_InstallmentsDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //CMS_MasterDepartmentDTO data = new CMS_MasterDepartmentDTO();
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return cms.loaddata(id);

        }
        [HttpPost]
        [Route("savedata")]
        public CMS_Master_InstallmentsDTO savedata([FromBody]CMS_Master_InstallmentsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_Master_InstallmentsDTO deactive([FromBody]CMS_Master_InstallmentsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(data);
        }
    }
}