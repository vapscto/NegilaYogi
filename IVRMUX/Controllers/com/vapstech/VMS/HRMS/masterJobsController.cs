using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;


namespace corewebapi18072016.Controllers.com.vapstech.VMS.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class masterJobsController : Controller
    {
        masterJobsDelegates del = new masterJobsDelegates();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Master_JobsDTO getalldetails(int id)
        {
            HR_Master_JobsDTO dto = new HR_Master_JobsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
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
        public HR_Master_JobsDTO Post([FromBody]HR_Master_JobsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Master_JobsDTO editRecord(int id)
        {
            HR_Master_JobsDTO dto = new HR_Master_JobsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public HR_Master_JobsDTO ActiveDeactiveRecord(int id)
        {
            HR_Master_JobsDTO dto = new HR_Master_JobsDTO();
            dto.HRMJ_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleterec(dto);
        }
    }
}
