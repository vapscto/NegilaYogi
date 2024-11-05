using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CLGHostelVacateReportFacadeController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public CLGHostelVacateReportInterface _objinter;

        public CLGHostelVacateReportFacadeController(CLGHostelVacateReportInterface para)
        {
            _objinter = para;
        }
        [Route("loaddata")]
        public CLGHostelVacateReportDTO loaddata([FromBody] CLGHostelVacateReportDTO data)
        {
            return _objinter.loaddata(data);
        }
        [Route("get_report")]
        public Task<CLGHostelVacateReportDTO> get_report([FromBody] CLGHostelVacateReportDTO data)
        {
            return _objinter.get_report(data);
        }
        [Route("get_Studentlist")]
        public CLGHostelVacateReportDTO get_Studentlist([FromBody] CLGHostelVacateReportDTO data)
        {
            return _objinter.get_Studentlist(data);
        }

    }
}
