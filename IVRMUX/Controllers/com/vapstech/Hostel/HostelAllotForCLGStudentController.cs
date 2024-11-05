using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Hostel
{
    [Route("api/[controller]")]
    public class HostelAllotForCLGStudentController : Controller
    {
        public HostelAllotForCLGStudentDelegate _delObj = new HostelAllotForCLGStudentDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata/{id:int}")]
        public HostelAllotForCLGStudentDTO loaddata(int id)
        {
            HostelAllotForCLGStudentDTO data = new HostelAllotForCLGStudentDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.loaddata(data);
        }

        [Route("savedata")]
        public HostelAllotForCLGStudentDTO savedata([FromBody] HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delObj.savedata(data);
        }

        [Route("get_studInfo")]
        public HostelAllotForCLGStudentDTO get_studInfo([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.get_studInfo(data);
        }
        [Route("floor")]
        public HostelAllotForCLGStudentDTO floor([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delObj.floor(data);
        }
        [Route("room")]
        public HostelAllotForCLGStudentDTO room([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delObj.room(data);
        }
        [Route("roomForVacateReport")]
        public HostelAllotForCLGStudentDTO roomForVacateReport([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delObj.roomForVacateReport(data);
        }
        [Route("roomdetails")]
        public HostelAllotForCLGStudentDTO roomdetails([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delObj.roomdetails(data);
        }

        [Route("get_roomdetails")]
        public HostelAllotForCLGStudentDTO get_roomdetails([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.get_roomdetails(data);
        }
        [Route("editdata")]
        public HostelAllotForCLGStudentDTO editdata([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delObj.editdata(data);
        }

        [Route("requestApproved")]
        public HostelAllotForCLGStudentDTO requestApproved([FromBody] HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.requestApproved(data);
        }

        [Route("requestRejected")]
        public HostelAllotForCLGStudentDTO requestRejected([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.requestRejected(data);
        }
        //HostelT
        [Route("HostelT")]
        public HL_Hostel_Student_Transfer_CollegeDTO HostelT([FromBody]HL_Hostel_Student_Transfer_CollegeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delObj.HostelT(data);
        }

        [Route("get_course")]
        public HostelAllotForCLGStudentDTO get_course([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delObj.get_course(data);
        }

        [Route("get_branch")]
        public HostelAllotForCLGStudentDTO get_branch([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delObj.get_branch(data);
        }
        [Route("get_sem")]
        public HostelAllotForCLGStudentDTO get_sem([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delObj.get_sem(data);
        }
        //[Route("get_sec")]
        //public HostelAllotForCLGStudentDTO get_sec([FromBody]HostelAllotForCLGStudentDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

        //    return _delObj.get_sec(data);
        //}
        [Route("get_student")]
        public HostelAllotForCLGStudentDTO get_student([FromBody]HostelAllotForCLGStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delObj.get_student(data);
        }
    }
}
