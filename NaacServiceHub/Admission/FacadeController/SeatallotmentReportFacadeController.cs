using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class SeatallotmentReportFacadeController : Controller
    {
        public SeatallotmentReportInterface _interface; 

        public SeatallotmentReportFacadeController(SeatallotmentReportInterface _intf)
        {
            _interface = _intf;
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
        public SeatallotmentReportDTO getdetails([FromBody] SeatallotmentReportDTO data)
        {           
            return _interface.getdetails(data);
        }
        [Route("onreport")]
        public SeatallotmentReportDTO onreport([FromBody]SeatallotmentReportDTO data)
        {
            return _interface.onreport(data);
        }

    }
}
