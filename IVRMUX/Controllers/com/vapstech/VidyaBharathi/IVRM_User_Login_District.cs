using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VidyaBharathi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VidyaBharathi
{
    [Route("api/[controller]")]
    public class IVRM_User_Login_District : Controller
    {
        IVRM_User_Login_DistrictDelegate cms = new IVRM_User_Login_DistrictDelegate();


        [HttpGet]
        [Route("loaddata/{id:int}")]
        public IVRM_User_Login_DistrictDTO loaddata(int id)
        {
            IVRM_User_Login_DistrictDTO dto = new IVRM_User_Login_DistrictDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.loaddata(dto);
        }
        [HttpPost]
        [Route("savedata")]
        public IVRM_User_Login_DistrictDTO savedata([FromBody]IVRM_User_Login_DistrictDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }

        [Route("deactive")]
        public IVRM_User_Login_DistrictDTO deactive([FromBody]IVRM_User_Login_DistrictDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.deactive(data);
        }
    }
}
