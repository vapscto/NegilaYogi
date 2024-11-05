using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class masterLeavingReasonFacade : Controller
    {
        masterLeavingReasonInterface inter;
        public masterLeavingReasonFacade(masterLeavingReasonInterface s)
        {
            inter = s;
        }
        // GET: api/<controller>
        [Route("loaddata")]
        public masterLeavingReasonDTO loaddata([FromBody]masterLeavingReasonDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public masterLeavingReasonDTO savedata([FromBody]masterLeavingReasonDTO data)
        {
            return inter.savedata(data);
        }
        [Route("EditData")]
        public masterLeavingReasonDTO EditData([FromBody]masterLeavingReasonDTO data)
        {
            return inter.EditData(data);
        }
        [Route("masterDecative")]
        public masterLeavingReasonDTO masterDecative([FromBody]masterLeavingReasonDTO data)
        {
            return inter.masterDecative(data);
        }
    }    
}
