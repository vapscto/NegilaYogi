using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;


namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SalaryAdvanceReportFacadeController : Controller
    {
        public SalaryadvanceInterfacereport _ads;

        public SalaryAdvanceReportFacadeController(SalaryadvanceInterfacereport adstu)
        {
            _ads = adstu;
        }
       
       
        [Route("onloadgetdetails")]
        public AdvanceReportDTO getinitialdata([FromBody]AdvanceReportDTO data)
        {
            return _ads.getBasicData(data);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<AdvanceReportDTO> getEmployeedetailsBySelection([FromBody]AdvanceReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }
        [Route("get_depts")]
        public AdvanceReportDTO get_depts([FromBody]AdvanceReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public AdvanceReportDTO get_desig([FromBody]AdvanceReportDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}
