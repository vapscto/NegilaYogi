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
    public class EmployeeOfferAndExperienceReportFacadeController : Controller
    {
        // GET: api/values
        public EmployeeOfferAndExperienceReportInterface _ads;

        public EmployeeOfferAndExperienceReportFacadeController(EmployeeOfferAndExperienceReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeOfferAndExperienceReportDTO getinitialdata([FromBody]EmployeeOfferAndExperienceReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public EmployeeOfferAndExperienceReportDTO FilterEmployeeData([FromBody]EmployeeOfferAndExperienceReportDTO dto)
        {
            return _ads.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<EmployeeOfferAndExperienceReportDTO> getEmployeedetailsBySelection([FromBody]EmployeeOfferAndExperienceReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }
    }
}
