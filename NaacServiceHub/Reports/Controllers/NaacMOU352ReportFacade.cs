using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Reports.Controllers
{
    [Route("api/[controller]")]
    public class NaacMOU352ReportFacade : Controller
    {
        public NaacMOU352ReportInterface _inter;

        public NaacMOU352ReportFacade(NaacMOU352ReportInterface y)
        {
            _inter = y;
        }

        [Route("getdata")]
        public NaacMOU352ReportDTO getdata([FromBody] NaacMOU352ReportDTO data)
        {
            return _inter.getdata(data);
        }
        [Route("get_report")]
        public Task<NaacMOU352ReportDTO> get_report([FromBody] NaacMOU352ReportDTO data)
        {
            return _inter.get_report(data);
        }

    }
}
