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
    public class GetVisitorReportFacadeController : Controller
    {
        GetVisitorReportInterface interobj;
        public GetVisitorReportFacadeController(GetVisitorReportInterface obj)
        {
            interobj = obj;
        }
       

       
        [HttpPost]
        [Route("report")]
        public Task<GetVisitorReportDTO> report([FromBody]GetVisitorReportDTO data)
        {
            return interobj.report(data);
        }
       
    }
}
