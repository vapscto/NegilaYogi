using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class HallTicketGenerationController : Controller
    {
        HallTicketGenerationDelegates _delobj = new HallTicketGenerationDelegates();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        [Route("getdetails/{id:int}")]
        public HallTicketGenerationDTO getdetails(HallTicketGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delobj.getdetails(data);
        }        

        [Route("onselectAcdYear")]
        public HallTicketGenerationDTO onselectAcdYear([FromBody]HallTicketGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public HallTicketGenerationDTO onselectclass([FromBody]HallTicketGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectclass(data);
        }

        [Route("onselectsection")]
        public HallTicketGenerationDTO onselectsection([FromBody]HallTicketGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectsection(data);
        }

        [Route("savedetail")]
        public HallTicketGenerationDTO savedetail([FromBody]HallTicketGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.savedetail(data);
        }

        [Route("ViewStudentDetails")]
        public HallTicketGenerationDTO ViewStudentDetails([FromBody]HallTicketGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.ViewStudentDetails(data);
        }

        [Route("SaveStudentStatus")]
        public HallTicketGenerationDTO SaveStudentStatus([FromBody]HallTicketGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.SaveStudentStatus(data);
        }
    }
}
