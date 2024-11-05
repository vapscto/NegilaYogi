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
    public class EmployeeDetailsReportFacadeController : Controller
    {
        public EmployeeDetailsReportInterface _ads;

        public EmployeeDetailsReportFacadeController(EmployeeDetailsReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmployeeReportsDTO getinitialdata([FromBody]EmployeeReportsDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("FilterEmployeeData")]
        public EmployeeReportsDTO FilterEmployeeData([FromBody]EmployeeReportsDTO dto)
        {
            return _ads.FilterEmployeeData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<EmployeeReportsDTO> getEmployeedetailsBySelection([FromBody]EmployeeReportsDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }


        [Route("get_depts")]
        public EmployeeReportsDTO get_depts([FromBody]EmployeeReportsDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public EmployeeReportsDTO get_desig([FromBody]EmployeeReportsDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}
