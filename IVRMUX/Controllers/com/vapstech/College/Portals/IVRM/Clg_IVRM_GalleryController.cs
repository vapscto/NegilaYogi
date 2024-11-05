using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using corewebapi18072016.Delegates.com.vapstech.Portals.IVRM;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class Clg_IVRM_GalleryController : Controller
    {
        Clg_IVRM_GalleryDelegate delgte = new Clg_IVRM_GalleryDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public ClgIVRMGalleryDTO getloaddata(int id)
        {
            ClgIVRMGalleryDTO data = new ClgIVRMGalleryDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.getloaddata(data);
        }

        [Route("get_branch")]
        public ClgIVRMGalleryDTO get_branch([FromBody]ClgIVRMGalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.get_branch(data);
        }

        [Route("get_semester")]
        public ClgIVRMGalleryDTO get_semester([FromBody]ClgIVRMGalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.get_semester(data);
        }

        [Route("get_Section")]
        public ClgIVRMGalleryDTO get_Section([FromBody]ClgIVRMGalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.get_Section(data);
        }

        [Route("savedata")]
        public ClgIVRMGalleryDTO savedata([FromBody]ClgIVRMGalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.savedata(data);
        }

        [Route("getcovermodel")]
        public ClgIVRMGalleryDTO getcovermodel([FromBody]ClgIVRMGalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.getcovermodel(data);
        }

        [Route("savecover")]
        public ClgIVRMGalleryDTO savecover([FromBody]ClgIVRMGalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delgte.savecover(data);
        }

        [Route("deactive")]
        public ClgIVRMGalleryDTO deactive([FromBody]ClgIVRMGalleryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delgte.deactive(data);
        }
    }
}
