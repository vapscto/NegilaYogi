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
    public class Alumni_NoticeBoardController : Controller
    {
        public Alumni_NoticeBoard_Delegate del = new Alumni_NoticeBoard_Delegate();

        [Route("loaddata/{id:int}")]
        public Alumni_NoticeBoard_DTO loaddata(int id)
        {
            Alumni_NoticeBoard_DTO data = new Alumni_NoticeBoard_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.loaddata(data);
        }

        [Route("savedetail")]
        public Alumni_NoticeBoard_DTO savedetail([FromBody] Alumni_NoticeBoard_DTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetail(data);
        }
         [Route("viewData")]
        public Alumni_NoticeBoard_DTO viewData([FromBody] Alumni_NoticeBoard_DTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.viewData(data);
        }
         [Route("deactivate")]
        public Alumni_NoticeBoard_DTO deactivate([FromBody] Alumni_NoticeBoard_DTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deactivate(data);
        }
         [Route("editdetails")]
        public Alumni_NoticeBoard_DTO editdetails([FromBody] Alumni_NoticeBoard_DTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.editdetails(data);
        }

    }
}