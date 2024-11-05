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
    public class V_AppointmentApprovalReportFacade : Controller
    {
      public V_AppointmentApprovalReportInterface interobj;
        public V_AppointmentApprovalReportFacade(V_AppointmentApprovalReportInterface obj)
        {
            interobj = obj;
        }
        
        [Route("loaddata")]
        public V_AppointmentApprovalReport_DTO loaddata([FromBody]V_AppointmentApprovalReport_DTO data)
        {
            return interobj.loaddata(data);
        }
         [Route("loaddatatoday")]
        public V_AppointmentApprovalReport_DTO loaddatatoday([FromBody]V_AppointmentApprovalReport_DTO data)
        {
            return interobj.loaddatatoday(data);
        }
        
        [Route("report")]
        public Task<V_AppointmentApprovalReport_DTO> report([FromBody]V_AppointmentApprovalReport_DTO data)
        {
            return interobj.report(data);
        }
    }
}
