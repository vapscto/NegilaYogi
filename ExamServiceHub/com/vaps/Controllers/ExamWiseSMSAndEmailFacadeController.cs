using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ExamWiseSMSAndEmailFacadeController : Controller
    {
        public ExamWiseSMSAndEmailInterface _interface;

        public ExamWiseSMSAndEmailFacadeController(ExamWiseSMSAndEmailInterface _inte)
        {
            _interface = _inte;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public ExamWiseSMSAndEmailDTO loaddata([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            return _interface.loaddata(data);
        }
        [Route("getclass")]
        public ExamWiseSMSAndEmailDTO getclass([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            return _interface.getclass(data);
        }
        [Route("getsection")]
        public ExamWiseSMSAndEmailDTO getsection([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            return _interface.getsection(data);
        }
        [Route("getexam")]
        public ExamWiseSMSAndEmailDTO getexam([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            return _interface.getexam(data);
        }
        [Route("searchDetails")]
        public ExamWiseSMSAndEmailDTO searchDetails([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            return _interface.searchDetails(data);
        }
        [Route("SendSmsEmail")]
        public ExamWiseSMSAndEmailDTO SendSmsEmail([FromBody]ExamWiseSMSAndEmailDTO data)
        {
            return _interface.SendSmsEmail(data);
        }
    }
}
