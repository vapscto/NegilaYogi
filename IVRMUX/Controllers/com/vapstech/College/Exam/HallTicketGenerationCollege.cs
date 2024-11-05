using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class HallTicketGenerationCollege : Controller
    {
        HallTicketGenerationCollegeDelgate _delobj = new HallTicketGenerationCollegeDelgate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        [Route("getdetails/{id:int}")]
        public HallTicketGenerationCollegeDTO getdetails(HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delobj.getdetails(data);
        }

        [Route("onselectAcdYear")]
        public HallTicketGenerationCollegeDTO onselectAcdYear([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public HallTicketGenerationCollegeDTO onselectclass([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectclass(data);
        }

        [Route("onselectsection")]
        public HallTicketGenerationCollegeDTO onselectsection([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectsection(data);
        }

        [Route("savedetail")]
        public HallTicketGenerationCollegeDTO savedetail([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.savedetail(data);
        }

        [Route("ViewStudentDetails")]
        public HallTicketGenerationCollegeDTO ViewStudentDetails([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.ViewStudentDetails(data);
        }

        [Route("SaveStudentStatus")]
        public HallTicketGenerationCollegeDTO SaveStudentStatus([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.SaveStudentStatus(data);
        }
        //ExamReport
        [Route("ExamReport")]
        public HallTicketGenerationCollegeDTO ExamReport([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMCST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            return _delobj.ExamReport(data);
        }
        //HalticketSubject
        [Route("HalticketSubject")]
        public HallTicketGenerationCollegeDTO HalticketSubject([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.HalticketSubject(data);
        }
        //savedetailHalticket
        [Route("savedetailHalticket")]
        public HallTicketGenerationCollegeDTO savedetailHalticket([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.savedetailHalticket(data);
        }
        //onedit
        [Route("onedit")]
        public HallTicketGenerationCollegeDTO onedit([FromBody]HallTicketGenerationCollegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onedit(data);
        }
    }
}
