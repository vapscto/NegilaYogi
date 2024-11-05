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
    public class CandidateListVMSController : Controller
    {
        CandidateListVMSDelegate del = new CandidateListVMSDelegate();
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
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRCD_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Candidate_DetailsDTO editRecord(int id)
        {
            //HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            //dto.HRMRFR_Id = id;
            return del.getRecorddetailsById(id);
        }

        //[Route("ActiveDeactiveRecord/{id:int}")]
        //public HR_Candidate_DetailsDTO ActiveDeactiveRecord(int id)
        //{
        //    HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
        //    dto.HRMET_Id = id;
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

        //    return del.deleterec(dto);
        //}
    }
}
