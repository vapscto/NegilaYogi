
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MeritListFacade : Controller
    {
        public MeritListInterface _examcateReport;
        public MeritListFacade(MeritListInterface data)
        {
            _examcateReport = data;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getdetails")]
        public MeritListDTO getdetails([FromBody]MeritListDTO data)//int IVRMM_Id
        {
            return _examcateReport.getdetails(data);
        }

        [Route("onchangeyear")]
        public MeritListDTO onchangeyear([FromBody] MeritListDTO data)
        {
            return _examcateReport.onchangeyear(data);
        }
        [Route("onchangeclass")]
        public MeritListDTO onchangeclass([FromBody] MeritListDTO data)
        {
            return _examcateReport.onchangeclass(data);
        }
        [Route("onchangesection")]
        public MeritListDTO onchangesection([FromBody] MeritListDTO data)
        {
            return _examcateReport.onchangesection(data);
        }

        [Route("getAttendetails")]
        public MeritListDTO getAttendetails([FromBody] MeritListDTO data)
        {
            return _examcateReport.getAttendetails(data);
        }
        [Route("getreport")]
        public MeritListDTO getreport([FromBody] MeritListDTO data)
        {
            return _examcateReport.getreport(data);
        }   

    }
}
