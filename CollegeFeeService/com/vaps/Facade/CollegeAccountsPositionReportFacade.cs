using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeAccountsPositionReportFacade : Controller
    {

        CollegeAccountsPositionReportInterface _interface;
        public CollegeAccountsPositionReportFacade(CollegeAccountsPositionReportInterface inter)
        {
            _interface = inter;
        }
        [Route("getdata")]
        public CollegeConcessionDTO getdata([FromBody] CollegeConcessionDTO data)
        {
            return _interface.getdata(data);
        }
        [Route("getgroupByCG")]
        public CollegeConcessionDTO getgroupByCG([FromBody] CollegeConcessionDTO data)
        {
            return _interface.getgroupByCG(data);
        }
        [Route("getReport")]
        public CollegeConcessionDTO getReport([FromBody] CollegeConcessionDTO data)
        {
            return _interface.getReport(data);
        }
    }
}
