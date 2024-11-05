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
    public class VBSC_Master_UOM : Controller
    {
        VBSC_Master_UOMDelegate cms = new VBSC_Master_UOMDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public VBSC_Master_UOMDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  IVRM_User_Login_StateDTO data = new IVRM_User_Login_StateDTO();
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return cms.loaddata(id);
        }

        [HttpPost]
        [Route("savedetails")]
        public VBSC_Master_UOMDTO savedetails([FromBody]VBSC_Master_UOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetails(data);
        }


        [Route("deactive")]
        public VBSC_Master_UOMDTO deactive([FromBody] VBSC_Master_UOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactive(data);
        }


        //competition level

        [HttpGet]
        [Route("getloaddatalevel/{id:int}")]
        public VBSC_Master_UOMDTO getloaddatalevel(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //  IVRM_User_Login_StateDTO data = new IVRM_User_Login_StateDTO();
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));            
            return cms.getloaddatalevel(id);
        }
        
        [HttpPost]
        [Route("savedetailslevel")]
        public VBSC_Master_UOMDTO savedetailslevel([FromBody]VBSC_Master_UOMDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedetailslevel(data);
        }

        [Route("deactivelevel")]
        public VBSC_Master_UOMDTO deactivelevel([FromBody] VBSC_Master_UOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.deactivelevel(data);
        }
 
    }

}





