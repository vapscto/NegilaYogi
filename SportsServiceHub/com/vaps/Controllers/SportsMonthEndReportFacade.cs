using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportsMonthEndReportFacade : Controller
    {
        // GET: api/<controller>
        SportsMonthEndReportInterface _interface;
        public SportsMonthEndReportFacade(SportsMonthEndReportInterface interfaces)
        {
            _interface = interfaces;
        }

        [Route("getdeatils")]
        public SportsMonthEndReport_DTO getdeatils([FromBody]SportsMonthEndReport_DTO data)
        {
            return _interface.getdeatils(data);
        }
        [Route("GetReport")]
        public SportsMonthEndReport_DTO GetReport([FromBody]SportsMonthEndReport_DTO data)
        {
            return _interface.GetReport(data);
        }


    }
}
