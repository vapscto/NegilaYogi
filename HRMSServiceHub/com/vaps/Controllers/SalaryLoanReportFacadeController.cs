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
    public class SalaryLoanReportFacadeController : Controller
    {
        public SalaryloanInterfacereport _ads;

        public SalaryLoanReportFacadeController(SalaryloanInterfacereport adstu)
        {
            _ads = adstu;
        }
       
       
        [Route("onloadgetdetails")]
        public LoanReportDTO getinitialdata([FromBody]LoanReportDTO data)
        {
            return _ads.getBasicData(data);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<LoanReportDTO> getEmployeedetailsBySelection([FromBody]LoanReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }
        [Route("get_depts")]
        public LoanReportDTO get_depts([FromBody]LoanReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public LoanReportDTO get_desig([FromBody]LoanReportDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}
