using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.VMS.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AddCandidateInterviewVMSController : Controller
    {
        AddCandidateInterviewVMSDelegate del = new AddCandidateInterviewVMSDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_CandidateInterviewScheduleDTO getalldetails(int id)
        {
            HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.onloadgetdetails(dto);
        }

        [HttpGet]
        [Route("getallgrade/{id:int}")]
        public HR_CandidateInterviewScheduleDTO getallgrade(int id)
        {
            HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.getallgrade(dto);
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
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_CandidateInterviewScheduleDTO editRecord(int id)
        {
          //  HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
           // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("getrpt")]
        public HR_CandidateInterviewScheduleDTO getrpt([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            //HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getrpt(dto);
        }

        [Route("savefeedback")]
        public HR_CandidateInterviewScheduleDTO savefeedback([FromBody]HR_CandidateInterviewScheduleDTO dto)
        {
            dto.HRCISC_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savefeedback(dto);
        }

    }
}
