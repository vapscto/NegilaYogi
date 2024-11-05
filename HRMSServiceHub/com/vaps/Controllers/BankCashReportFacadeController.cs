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
    public class BankCashReportFacadeController : Controller
    {
        // GET: api/values
        public BankCashReportInterface _ads;

        public BankCashReportFacadeController(BankCashReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public BankCashReportDTO getinitialdata([FromBody]BankCashReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getEmployeedetailsBySelection")]
        public Task<BankCashReportDTO> getEmployeedetailsBySelection([FromBody]BankCashReportDTO dto)
        {
            return _ads.getEmployeedetailsBySelection(dto);
        }

        [Route("get_depts")]
        public BankCashReportDTO get_depts([FromBody]BankCashReportDTO dto)
        {
            return _ads.get_depts(dto);
        }
        [Route("get_desig")]
        public BankCashReportDTO get_desig([FromBody]BankCashReportDTO dto)
        {
            return _ads.get_desig(dto);
        }
    }
}
