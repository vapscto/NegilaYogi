using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class PeriodwseleavReportFacadeController : Controller
    {
        public PeriodWseLeavReportInterface _org;

        public PeriodwseleavReportFacadeController(PeriodWseLeavReportInterface maspag)
        {
            _org = maspag;
        }
        // [HttpPost]
        [Route("getalldetails")]
        public LeaveCreditDTO Getdet([FromBody] LeaveCreditDTO data)
        {
            return _org.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO data)
        {
            return _org.get_departments(data);
        }
        [HttpPost]
        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO data)
        {
            return _org.get_designation(data);
        }

        [HttpPost]
        [Route("get_employee")]
        public LeaveCreditDTO get_employee([FromBody] LeaveCreditDTO data)
        {
            return _org.get_employee(data);
        }

        [Route("getrpt")]
        public LeaveCreditDTO getrpt([FromBody] LeaveCreditDTO org)
        {
            return  _org.getreport(org);
        }
        [Route("getsiglerpt")]
        public LeaveCreditDTO getsiglerpt([FromBody] LeaveCreditDTO org)
        {
            return _org.getsiglerpt(org);
        }
    }
}