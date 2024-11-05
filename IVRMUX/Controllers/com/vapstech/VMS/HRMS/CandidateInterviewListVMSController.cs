using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CandidateInterviewListVMSController : Controller
    {
        CandidateInterviewListVMSDelegate del = new CandidateInterviewListVMSDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_CandidateInterviewScheduleDTO getalldetails(int id)
        {
            HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public HR_CandidateInterviewScheduleDTO Post([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRCISC_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_CandidateInterviewScheduleDTO editRecord(int id)
        {
            //HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            //dto.HRMRFR_Id = id;
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public HR_CandidateInterviewScheduleDTO ActiveDeactiveRecord(int id)
        {
            HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.HRCISC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleterec(dto);
        }

        [HttpGet]
        [Route("getallwithoutcondtn/{id:int}")]
        public HR_CandidateInterviewScheduleDTO getallwithoutcondtn(int id)
        {
            HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getallwithoutcondtn(dto);
        }
        
    }
}
