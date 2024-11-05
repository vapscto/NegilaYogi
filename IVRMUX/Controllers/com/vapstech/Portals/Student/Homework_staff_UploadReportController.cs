using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace IVRMUX.Controllers.com.vapstech.Portals.IVRM
{
    [Produces("application/json")]
    [Route("api/Homework_staff_UploadReport")]
    public class Homework_staff_UploadReportController : Controller
    {
        HomeworkStaffReportDelegate LMD = new HomeworkStaffReportDelegate();
        [HttpGet]
        [Route("getAllDetail/{id:int}")]
        public HomeworkStaffReportDTO getAllDetail(int id)
        {
            HomeworkStaffReportDTO data = new HomeworkStaffReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getAllDetail(data);
        }
        [Route("get_load_onchange")]
        public HomeworkStaffReportDTO get_load_onchane([FromBody] HomeworkStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.get_load_onchange(dto);
        }
        [HttpPost]
        [Route("getReport")]
        public HomeworkStaffReportDTO getReport([FromBody] HomeworkStaffReportDTO dto)
        {            
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getReport(dto);
        }
        //getOnchange
        [Route("getOnchange")]
        public HomeworkStaffReportDTO getOnchange([FromBody] HomeworkStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getOnchange(dto);
        }
        //------------Class-Wise-Report------------//
        [HttpGet]
        [Route("getloadDetails/{id:int}")]
        public HomeworkStaffReportDTO getloadDetails(int id)
        {
            HomeworkStaffReportDTO data = new HomeworkStaffReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.USER_ID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleType = HttpContext.Session.GetString("RoleNme");           
            return LMD.getloadDetails(data);
        }
        [Route("getLoad_onchange")]
        public HomeworkStaffReportDTO getLoad_onchange([FromBody] HomeworkStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return LMD.getLoad_onchange(dto);
        }
        [HttpPost]
        [Route("getReport_classwise")]
        public HomeworkStaffReportDTO getReport_classwise([FromBody]HomeworkStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleType = HttpContext.Session.GetString("RoleNme");
            dto.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getReport_classwise(dto);
        }
        //getOnchangeclass
        [Route("getOnchangeclass")]
        public HomeworkStaffReportDTO getOnchangeclass([FromBody] HomeworkStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getOnchangeclass(dto);
        }
        //======sms email========
        //smsemail
        [Route("smsemail")]
        public HomeworkStaffReportDTO smsemail([FromBody]HomeworkStaffReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleType = HttpContext.Session.GetString("RoleNme");
            return LMD.smsemail(dto);
        }
       
    }
}