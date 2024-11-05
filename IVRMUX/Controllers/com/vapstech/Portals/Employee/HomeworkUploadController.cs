using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace IVRMUX.Controllers.com.vapstech.Portals.Employee
{

    [Route("api/[controller]")]
    public class HomeworkUploadController : Controller
    {
        HomeworkUploadDelegate updel = new HomeworkUploadDelegate();


        [Route("Getdata_class/{id:int}")]
        public HomeWorkUploadDTO Getdata_class(int id)
        {
            HomeWorkUploadDTO dto = new HomeWorkUploadDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return updel.Getdata_class(dto);
        }
        [Route("Getdataview")]
        public HomeWorkUploadDTO Getdataview([FromBody] HomeWorkUploadDTO dto)
        {
           // HomeWorkUploadDTO dto = new HomeWorkUploadDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return updel.Getdataview(dto);
        }
        [Route("getreport_class")]
        public HomeWorkUploadDTO getreport_class([FromBody] HomeWorkUploadDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return updel.getreport_class(dto);
        }
        [Route("getreport_home")]
        public HomeWorkUploadDTO getreport_home([FromBody] HomeWorkUploadDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return updel.getreport_home(dto);
        }
      
        [Route("getreport_notice")]
        public HomeWorkUploadDTO getreport_notice([FromBody] HomeWorkUploadDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return updel.getreport_notice(dto);
        }

        [Route("getsection")]
        public HomeWorkUploadDTO getsection([FromBody]HomeWorkUploadDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return updel.getsection(data);
        }

        [Route("getseenreport")]
        public HomeWorkUploadDTO getseenreport([FromBody]HomeWorkUploadDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return updel.getseenreport(data);
        }

        [Route("Getdataview_seen")]
        public HomeWorkUploadDTO Getdataview_seen([FromBody] HomeWorkUploadDTO dto)
        {
            // HomeWorkUploadDTO dto = new HomeWorkUploadDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return updel.Getdataview_seen(dto);
        }

        [Route("viewData")]
        public HomeWorkUploadDTO viewData([FromBody] HomeWorkUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return updel.viewData(data);
        }
    }
}