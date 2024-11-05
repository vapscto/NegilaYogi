using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class NAACReportFacadeController : Controller
    {
        public NAACReportInterface inter;
        public NAACReportFacadeController (NAACReportInterface obj)
        {
            inter = obj;
        }

        [Route("getdetails")]
        public NAACReportDTO Getdetails([FromBody] NAACReportDTO data)
        {
            return inter.getdetails(data);
        }

        [Route("onreport")]
        public NAACReportDTO onreport([FromBody] NAACReportDTO data)
        {
            return inter.onreport(data);
        }
    }
}
