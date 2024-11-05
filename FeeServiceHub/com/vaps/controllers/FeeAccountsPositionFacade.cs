using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeAccountsPositionFacade : Controller
    {
        FeeAccountsPositionInterface _interface;
        public FeeAccountsPositionFacade(FeeAccountsPositionInterface inter)
        {
            _interface = inter;
        }
       [Route("getdata")]
       public FeeAccountsPositionReportDTO getdata([FromBody] FeeAccountsPositionReportDTO data)
        {
            return _interface.getdata(data);
        }
        [Route("getgroupByCG")]
        public FeeAccountsPositionReportDTO getgroupByCG([FromBody] FeeAccountsPositionReportDTO data)
        {
            return _interface.getgroupByCG(data);
        }
        [Route("getReport")]
        public FeeAccountsPositionReportDTO getReport([FromBody] FeeAccountsPositionReportDTO data)
        {
            return _interface.getReport(data);
        }
    }
}
