using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServicesHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HeadwiseReportsFacadeController : Controller
    {
        // GET: api/values
        public HeadwiseReportsInterface _ads;

        public HeadwiseReportsFacadeController(HeadwiseReportsInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HeaderwiseReportDTO getinitialdata([FromBody]HeaderwiseReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<HeaderwiseReportDTO> getEmployeedetailsBySelection([FromBody]HeaderwiseReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("get_depts")]
        public HeaderwiseReportDTO get_depts([FromBody]HeaderwiseReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public HeaderwiseReportDTO get_desig([FromBody]HeaderwiseReportDTO dto)
        {
            return _ads.get_desig(dto);
        }

    }
}
