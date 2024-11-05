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
    public class NaacAward342ReportFacade : Controller
    {
        public NaacAward342ReportInterface _inter;

public NaacAward342ReportFacade(NaacAward342ReportInterface y)
        {
            _inter = y;
        }

        [Route("getdata")]
        public NaacAward342ReportDTO getdata([FromBody] NaacAward342ReportDTO data)
        {
            return _inter.getdata(data);
        }
        [Route("get_report")]
        public Task<NaacAward342ReportDTO> get_report([FromBody] NaacAward342ReportDTO data)
        {
            return _inter.get_report(data);
        }

    }
}
