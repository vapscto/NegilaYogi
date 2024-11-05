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
    public class LeftStudentsReportFacade : Controller
    {
        public LeftStudentsReportInterface _interface;

        public LeftStudentsReportFacade(LeftStudentsReportInterface _inter)
        {
            _interface = _inter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public LeftStudentsReportDTO loaddata([FromBody] LeftStudentsReportDTO data)
        {
            return _interface.loaddata(data);
        }
        //getCategory
        [Route("getCategory")]
        public LeftStudentsReportDTO getCategory([FromBody] LeftStudentsReportDTO data)
        {
            return _interface.getCategory(data);
        }
        //getClass
        [Route("getClass")]
        public LeftStudentsReportDTO getClass([FromBody] LeftStudentsReportDTO data)
        {
            return _interface.getClass(data);
        }
        //getsection
        [Route("getsection")]
        public LeftStudentsReportDTO getsection([FromBody] LeftStudentsReportDTO data)
        {
            return _interface.getsection(data);
        }
        //report
        [Route("report")]
        public LeftStudentsReportDTO report([FromBody] LeftStudentsReportDTO data)
        {
            return _interface.report(data);
        }
    }
}
