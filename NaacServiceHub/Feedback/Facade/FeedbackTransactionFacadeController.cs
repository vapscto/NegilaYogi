using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.FeedBack;
using NaacServiceHub.FeedBack.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.FeedBack.Facade
{
    [Route("api/[controller]")]
    public class FeedbackTransactionFacadeController : Controller
    {
        public FeedbackTransactionInterface _interface;

        public FeedbackTransactionFacadeController(FeedbackTransactionInterface intf)
        {
            _interface = intf;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Route("getdetails")]
        public FeedbackTransactionDTO getdetails([FromBody]FeedbackTransactionDTO data)
        {
            return _interface.getdetails(data);
        }
        [Route("getfeedback")]
        public FeedbackTransactionDTO getfeedback([FromBody]FeedbackTransactionDTO data)
        {
            return _interface.getfeedback(data);
        }
        
        [Route("savedata")]
        public FeedbackTransactionDTO savedata([FromBody] FeedbackTransactionDTO data)
        {   
            return _interface.savedata(data);
        }


        [Route("getstudentstaffdetails")]
        public FeedbackTransactionDTO getstudentstaffdetails([FromBody] FeedbackTransactionDTO data)
        {
            return _interface.getstudentstaffdetails(data);
        }
        [Route("getstaffname")]
        public FeedbackTransactionDTO getstaffname([FromBody] FeedbackTransactionDTO data)
        {
            return _interface.getstaffname(data);
        }
        [Route("getfeedbacksubject")]
        public FeedbackTransactionDTO getfeedbacksubject([FromBody] FeedbackTransactionDTO data)
        {
            return _interface.getfeedbacksubject(data);
        }
        
        [Route("studentstaffsavedata")]
        public FeedbackTransactionDTO studentstaffsavedata([FromBody] FeedbackTransactionDTO data)
        {
            return _interface.studentstaffsavedata(data);
        }

        // Feedbcak Modulewise 17-02-2024
        [Route("modulewisefeedback")]
        public FeedbackTransactionDTO modulewisefeedback([FromBody] FeedbackTransactionDTO data)
        {
            return _interface.modulewisefeedback(data);
        }

        [Route("loadfeedbackquestion")]
        public FeedbackTransactionDTO loadfeedbackquestion([FromBody] FeedbackTransactionDTO data)
        {
            return _interface.loadfeedbackquestion(data);
        }
    }
}
