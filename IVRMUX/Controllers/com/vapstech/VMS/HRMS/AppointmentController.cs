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
    public class AppointmentController : Controller
    {
        AppointmentDelegate del = new AppointmentDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Candidate_DetailsDTO getalldetails(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
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
        public HR_Candidate_DetailsDTO Post([FromBody]HR_Candidate_DetailsDTO dto)
        {
           // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRCD_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Candidate_DetailsDTO editRecord(int id)
        {
            return del.getRecorddetailsById(id);
        }

        [Route("Get_Desgination")]
        public HR_Candidate_DetailsDTO Get_Desgination([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Get_Desgination(data);
        }
        [Route("saveAppointmentdata")]
        public HR_Candidate_DetailsDTO saveAppointmentdata([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.Employeedto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.saveAppointmentdata(data);
        }

        
        [Route("savesalarydata")]
        public HR_Candidate_DetailsDTO savesalarydata([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Employeedto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savesalarydata(data);
        }

        [Route("getEmployeeSalaryDetailsByHead")]
        public HR_Candidate_DetailsDTO getEmployeeSalaryDetailsByHead([FromBody] HR_Candidate_DetailsDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getEmployeeSalaryDetailsByHead(data);
        }

        [Route("getcandidate")]
        public HR_Candidate_DetailsDTO getcandidate([FromBody] HR_Candidate_DetailsDTO data)
        {
            //HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getcandidate(data);
        }

        [Route("getCandidateList/{id:int}")]
        public HR_Candidate_DetailsDTO getCandidateList(int id)
        {
            HR_Candidate_DetailsDTO data = new HR_Candidate_DetailsDTO();
           data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getCandidateList(data);
        }    

        [Route("getcandidatename")]
        public HR_Candidate_DetailsDTO getcandidatename([FromBody] HR_Candidate_DetailsDTO data)
        {
            //HR_Candidate_DetailsDTO data = new HR_Candidate_DetailsDTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcandidatename(data);
        }

        [Route("getcompanydetails")]
        public HR_Candidate_DetailsDTO getcompanydetails([FromBody] HR_Candidate_DetailsDTO data)
        {
            return del.getcompanydetails(data);
        }
    }
}
