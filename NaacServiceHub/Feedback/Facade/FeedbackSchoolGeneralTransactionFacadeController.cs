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
    public class FeedbackSchoolGeneralTransactionFacadeController : Controller
    {
        public FeedbackSchoolGeneralTransactionInterface _interface;

        public FeedbackSchoolGeneralTransactionFacadeController(FeedbackSchoolGeneralTransactionInterface intf)
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
        public FeedbackSchoolGeneralTransactionDTO getdetails([FromBody]FeedbackSchoolGeneralTransactionDTO data)
        {
            return _interface.getdetails(data);
        }
        [Route("getfeedback")]
        public FeedbackSchoolGeneralTransactionDTO getfeedback([FromBody]FeedbackSchoolGeneralTransactionDTO data)
        {
            return _interface.getfeedback(data);
        }
        
        [Route("savedata")]
        public FeedbackSchoolGeneralTransactionDTO savedata([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            return _interface.savedata(data);
        }

        [Route("getstudentstaffdetails")]
        public FeedbackSchoolGeneralTransactionDTO getstudentstaffdetails([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            return _interface.getstudentstaffdetails(data);
        }
        [Route("getstaffname")]
        public FeedbackSchoolGeneralTransactionDTO getstaffname([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            return _interface.getstaffname(data);
        }
        [Route("getfeedbacksubject")]
        public FeedbackSchoolGeneralTransactionDTO getfeedbacksubject([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            return _interface.getfeedbacksubject(data);
        }
        
        [Route("studentstaffsavedata")]
        public FeedbackSchoolGeneralTransactionDTO studentstaffsavedata([FromBody] FeedbackSchoolGeneralTransactionDTO data)
        {
            return _interface.studentstaffsavedata(data);
        }
    }
}
