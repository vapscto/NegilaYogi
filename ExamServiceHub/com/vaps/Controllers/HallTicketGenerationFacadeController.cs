using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HallTicketGenerationFacadeController : Controller
    {
        private HallTicketGenerationInterface _inter;
        public HallTicketGenerationFacadeController (HallTicketGenerationInterface obj)
        {
            _inter = obj;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getdetails")]
        public HallTicketGenerationDTO Getdetails([FromBody] HallTicketGenerationDTO data)
        {
            return _inter.getdetails(data);
        }

        [Route("onselectAcdYear")]
        public HallTicketGenerationDTO onselectAcdYear([FromBody] HallTicketGenerationDTO data)
        {
            return _inter.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public HallTicketGenerationDTO onselectclass([FromBody] HallTicketGenerationDTO data)
        {
            return _inter.onselectclass(data);
        }
        [Route("onselectsection")]
        public HallTicketGenerationDTO onselectsection([FromBody] HallTicketGenerationDTO data)
        {
            return _inter.onselectsection(data);
        }

        [Route("savedetail")]
        public HallTicketGenerationDTO savedetail([FromBody] HallTicketGenerationDTO data)
        {
            return _inter.savedetail(data);
        }

        [Route("ViewStudentDetails")]
        public HallTicketGenerationDTO ViewStudentDetails([FromBody] HallTicketGenerationDTO data)
        {
            return _inter.ViewStudentDetails(data);
        }

        [Route("SaveStudentStatus")]
        public HallTicketGenerationDTO SaveStudentStatus([FromBody] HallTicketGenerationDTO data)
        {
            return _inter.SaveStudentStatus(data);
        }
    }
}
