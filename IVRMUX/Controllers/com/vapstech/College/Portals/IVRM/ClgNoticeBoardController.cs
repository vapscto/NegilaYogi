using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.IVRM;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgNoticeBoardController : Controller
    {
        ClgNoticeBoardDelegate clgdelegate = new ClgNoticeBoardDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgNoticeBoardDTO getloaddata(ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));                               
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getloaddata(data);
        }

        [Route("getbranchdata")]
        public ClgNoticeBoardDTO getbranchdata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getbranchdata(data);
        }

        [Route("getsemdata")]
        public ClgNoticeBoardDTO getsemdata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getsemdata(data);
        }
        [Route("savedata")]
        public ClgNoticeBoardDTO savedata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return clgdelegate.savedata(data);
        }
        [Route("getNoticedata")]
        public ClgNoticeBoardDTO getNoticedata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getNoticedata(data);
        }

        [Route("editdetails")]
        public ClgNoticeBoardDTO editdetails([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.editdetails(data);
        }
        [Route("deactive")]
        public ClgNoticeBoardDTO deactive([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.deactive(data);
        }
        [Route("deactivedetails")]
        public ClgNoticeBoardDTO deactivedetails([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.deactivedetails(data);
        }
        [Route("Getdata_class/{id:int}")]
        public ClgNoticeBoardDTO Getdata_class(int id)
        {
            ClgNoticeBoardDTO dto = new ClgNoticeBoardDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return clgdelegate.Getdata_class(dto);
        }
        [Route("getreportnotice")]
        public ClgNoticeBoardDTO getreportnotice([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getreportnotice(data);
        }
        [Route("Getdataview")]
        public ClgNoticeBoardDTO Getdataview([FromBody] ClgNoticeBoardDTO dto)
        {
            // HomeWorkUploadDTO dto = new HomeWorkUploadDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return clgdelegate.Getdataview(dto);
        }
        [Route("getstudent")]
        public ClgNoticeBoardDTO getstudent([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return clgdelegate.getstudent(data);
        }

        //course
        [Route("getcoursedata")]
        public ClgNoticeBoardDTO getcoursedata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getcoursedata(data);
        }

        //Akash
        [Route("Deptselectiondetails")]
        public ClgNoticeBoardDTO Deptselectiondetails([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return clgdelegate.Deptselectiondetails(data);
        }

        [Route("Desgselectiondetails")]
        public ClgNoticeBoardDTO Desgselectiondetails([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return clgdelegate.Desgselectiondetails(data);
        }
    }
}
