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
    public class GatePassFacadeController : Controller
    {
        GatePassInterface interobj;
        public GatePassFacadeController(GatePassInterface obj)
        {
            interobj = obj;
        }
       
       
        [HttpPost]
        [Route("getDetails")]
        public GatePassDTO getDetails([FromBody]GatePassDTO data)
        {
            return interobj.getDetails(data);
        }
        [Route("saveData")]
        public GatePassDTO saveData([FromBody]GatePassDTO data)
        {
            return interobj.saveData(data);
        }
        [Route("getStudentDetails")]
        public GatePassDTO getStudentDetails([FromBody]GatePassDTO data)
        {
            return interobj.getStudentDetails(data);
        }
        [Route("sendmail")]
        public GatePassDTO sendmail([FromBody]GatePassDTO data)
        {
            return interobj.sendmail(data);
        }

    }
}
