
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlacementServiceHub.com.Interfaces;
using PreadmissionDTOs.com.vaps.Placement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.DriversChart.Facade
{
    [Route("api/[controller]")]
    public class CISReportFacade : Controller
    {
        public CISReportInterface inter;
        public CISReportFacade(CISReportInterface s)
        {
            inter = s;
        }
        [Route("getdetails")]
        public PL_CampusInterview_ScheduleDTO getdetails([FromBody] PL_CampusInterview_ScheduleDTO data)
        {
            return inter.getdetails(data);
        }
        [Route("report")]
        public PL_CampusInterview_ScheduleDTO report([FromBody] PL_CampusInterview_ScheduleDTO data)
        {
            return inter.report(data);
        }



    }
}
