using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class MonthEndReportFacade : Controller
    {

        MonthEndReportInterface _interface;
        public MonthEndReportFacade(MonthEndReportInterface interfaces)
        {
            _interface = interfaces;
        }

        [Route("getdeatils")]
        public VisitorsMonthEndReport_DTO getdeatils([FromBody]VisitorsMonthEndReport_DTO data)
        {
            return _interface.getdeatils(data);
        }
        [Route("GetReport")]
        public Task<VisitorsMonthEndReport_DTO> GetReport([FromBody]VisitorsMonthEndReport_DTO data)
        {
            return _interface.GetReport(data);
        }

    }
}
