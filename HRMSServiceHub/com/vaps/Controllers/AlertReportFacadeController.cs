using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServicesHub.com.vaps.Interfaces;

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AlertReportFacadeController : Controller
    {
        public AlertReportInterface _ads;

        public AlertReportFacadeController(AlertReportInterface adstu)
        {
            _ads = adstu;
        }

        [Route("getAlertReport")]
        public Task<MasterEmployeeDTO> getAlertReport([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.getAlertReport(dto);
        }
    }
}
