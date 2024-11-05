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
    public class EmployeeContributionReportFacadeController : Controller
    {
        // GET: api/values
        public EmployeeContributionReportInterface _ads;

        public EmployeeContributionReportFacadeController(EmployeeContributionReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeContributionReportDTO getinitialdata([FromBody]EmployeeContributionReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public EmployeeContributionReportDTO FilterEmployeeData([FromBody]EmployeeContributionReportDTO dto)
        {
            return _ads.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public EmployeeContributionReportDTO getEmployeedetailsBySelection([FromBody]EmployeeContributionReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }


        [Route("get_depts")]
        public EmployeeContributionReportDTO get_depts([FromBody]EmployeeContributionReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public EmployeeContributionReportDTO get_desig([FromBody]EmployeeContributionReportDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}
