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
    public class HostelVacateReportFacade : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public HostelVacateReportInterface _objinter;

        public HostelVacateReportFacade(HostelVacateReportInterface para)
        {
            _objinter = para;
        }
        [Route("loaddata")]
        public HostelVacateReportDTO loaddata([FromBody] HostelVacateReportDTO data)
        {
            return _objinter.loaddata(data);
        }
        [Route("get_report")]
        public Task<HostelVacateReportDTO> get_report([FromBody] HostelVacateReportDTO data)
        {
            return _objinter.get_report(data);
        }
        [Route("get_Studentlist")]
        public HostelVacateReportDTO get_Studentlist([FromBody] HostelVacateReportDTO data)
        {
            return _objinter.get_Studentlist(data);
        }

    }
}
