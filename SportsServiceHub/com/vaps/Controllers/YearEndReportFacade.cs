using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class YearEndReportFacade : Controller
    {
        YearEndReportInterface _interfac;
        public YearEndReportFacade(YearEndReportInterface interfac)
        {
            _interfac = interfac;
        }
        [Route("loadDrpDwn")]
        public YearEndReportDTO loadDrpDwn([FromBody]YearEndReportDTO data)
        {
            return _interfac.loadDrpDwn(data);
        }
        [Route("getReport")]
        public Task<YearEndReportDTO> getReport([FromBody]YearEndReportDTO obj)
        {
            return _interfac.getReport(obj);
        }
        [Route("getReportGraph")]
        public YearEndReportDTO getReportGraph([FromBody]YearEndReportDTO obj)
        {
            return _interfac.getReportGraph(obj);
        }
        
    }
}
