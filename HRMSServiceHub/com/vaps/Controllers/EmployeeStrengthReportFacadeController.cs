using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeStrengthReportFacadeController : Controller
    {
        // GET: api/values
        public EmployeeStrengthReportInterface _ads;

        public EmployeeStrengthReportFacadeController(EmployeeStrengthReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeStrengthReportDTO getinitialdata([FromBody]EmployeeStrengthReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public EmployeeStrengthReportDTO getEmployeedetailsBySelection([FromBody]EmployeeStrengthReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("get_depts")]
        public EmployeeStrengthReportDTO get_depts([FromBody]EmployeeStrengthReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public EmployeeStrengthReportDTO get_desig([FromBody]EmployeeStrengthReportDTO dto)
        {
            return _ads.get_desig(dto);
        }

    }
}
