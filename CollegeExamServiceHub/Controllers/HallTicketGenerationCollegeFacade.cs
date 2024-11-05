using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeExamServiceHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeExamServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class HallTicketGenerationCollegeFacade : Controller
    {
        private HallTicketGenerationCollege _inter;
        public HallTicketGenerationCollegeFacade(HallTicketGenerationCollege obj)
        {
            _inter = obj;
        }
        [Route("getdetails")]
        public HallTicketGenerationCollegeDTO Getdetails([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onselectAcdYear")]
        public HallTicketGenerationCollegeDTO onselectAcdYear([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public HallTicketGenerationCollegeDTO onselectclass([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.onselectclass(data);
        }
        [Route("onselectsection")]
        public HallTicketGenerationCollegeDTO onselectsection([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.onselectsection(data);
        }

        [Route("savedetail")]
        public HallTicketGenerationCollegeDTO savedetail([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.savedetail(data);
        }

        [Route("ViewStudentDetails")]
        public HallTicketGenerationCollegeDTO ViewStudentDetails([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.ViewStudentDetails(data);
        }

        [Route("SaveStudentStatus")]
        public HallTicketGenerationCollegeDTO SaveStudentStatus([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.SaveStudentStatus(data);
        }
        //ExamReport
        [Route("ExamReport")]
        public HallTicketGenerationCollegeDTO ExamReport([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.ExamReport(data);
        }
        //HalticketSubject
        [Route("HalticketSubject")]
        public HallTicketGenerationCollegeDTO HalticketSubject([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.HalticketSubject(data);
        }
        //savedetailHalticket
        [Route("savedetailHalticket")]
        public HallTicketGenerationCollegeDTO savedetailHalticket([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.savedetailHalticket(data);
        }
        //onedit
        [Route("onedit")]
        public HallTicketGenerationCollegeDTO onedit([FromBody] HallTicketGenerationCollegeDTO data)
        {
            return _inter.onedit(data);
        }
    }
}
