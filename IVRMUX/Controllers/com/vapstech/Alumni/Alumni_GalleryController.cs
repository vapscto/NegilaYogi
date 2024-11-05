using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Alumni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

namespace IVRMUX.Controllers.com.vapstech.Alumni
{
   
    [Route("api/[controller]")]
    public class Alumni_GalleryController : Controller
    {
        public Alumni_Gallery_Delegate delgte = new Alumni_Gallery_Delegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public Alumni_GalleryDTO getloaddata(int id)
        {
            Alumni_GalleryDTO data = new Alumni_GalleryDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return delgte.getloaddata(data);
        }

       
        [Route("savedata")]
        public Alumni_GalleryDTO savedata([FromBody]Alumni_GalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.savedata(data);
        }

        [Route("getcovermodel")]
        public Alumni_GalleryDTO getcovermodel([FromBody]Alumni_GalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           // data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.getcovermodel(data);
        }

        [Route("savecover")]
        public Alumni_GalleryDTO savecover([FromBody]Alumni_GalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           // data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.savecover(data);
        }
        [Route("deactive")]
        public Alumni_GalleryDTO deactive([FromBody]Alumni_GalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delgte.deactive(data);
        }


    }
}