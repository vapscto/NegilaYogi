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
    public class MonthEndReportFacadeController : Controller
    {
        // GET: api/values
        public MonthEndReportInterface _ads;

        public MonthEndReportFacadeController(MonthEndReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public MonthEndReportDTO getinitialdata([FromBody]MonthEndReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<MonthEndReportDTO> getEmployeedetailsBySelection([FromBody]MonthEndReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }
    }
}
