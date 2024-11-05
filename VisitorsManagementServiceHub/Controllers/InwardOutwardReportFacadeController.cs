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
    public class InwardOutwardReportFacadeController : Controller
    {
        InwardOutwardReportInterface interobj;
        public InwardOutwardReportFacadeController(InwardOutwardReportInterface obj)
        {
            interobj = obj;
        }
       
       
        [HttpPost]
        [Route("report")]
        public Task<InwardOutwardReportDTO> report([FromBody]InwardOutwardReportDTO data)
        {
            return interobj.report(data);
        }
       
       
    }
}
