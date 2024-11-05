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
    public class GatePassReportFacadeController : Controller
    {
        GatePassReportInterface interobj;
        public GatePassReportFacadeController(GatePassReportInterface obj)
        {
            interobj = obj;
        }
     
        [HttpPost]
        [Route("report")]
        public Task<GatePassReportDTO> report([FromBody]GatePassReportDTO data)
        {
            return interobj.report(data);
        }

        [Route("loaddata")]
        public GatePassReportDTO loaddata([FromBody] GatePassReportDTO data)
        {
            return interobj.loaddata(data);
        }

        [Route("reportforMobile")]
        public Task<GatePassReportDTO> reportforMobile([FromBody] GatePassReportDTO data)
        {
            return interobj.reportforMobile(data);
        }

    }
}
