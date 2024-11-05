using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ECSReportFacadeController : Controller
    {
        public ECSReportInterface _interface;

        public ECSReportFacadeController(ECSReportInterface _inte)
        {
            _interface = _inte;
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

        [Route("getloaddata")]
        public ECSReportDTO getloaddata([FromBody] ECSReportDTO data)
        {            
            return _interface.getloaddata(data);
        }

        [Route("getclass")]
        public ECSReportDTO getclass([FromBody] ECSReportDTO data)
        {
            return _interface.getclass(data);
        }
        [Route("getsection")]
        public ECSReportDTO getsection([FromBody] ECSReportDTO data)
        {
            return _interface.getsection(data);
        }
        [Route("getreport")]
        public ECSReportDTO getreport([FromBody] ECSReportDTO data)
        {
            return _interface.getreport(data);
        }
        [Route("showecsdetails")]
        public ECSReportDTO showecsdetails([FromBody] ECSReportDTO data)
        {
            return _interface.showecsdetails(data);
        }
        [Route("searchByColumn")]
        public ECSReportDTO searchByColumn([FromBody] ECSReportDTO data)
        {
            return _interface.searchByColumn(data);
        }

    }
}
