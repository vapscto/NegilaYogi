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
    public class masterSpecialisationFacade : Controller
    {
        masterSpecialisationInterface inter;
        public masterSpecialisationFacade(masterSpecialisationInterface s)
        {
            inter = s;
        }
        // GET: api/<controller>
        [Route("loaddata")]
        public masterSpecialisationDTO loaddata([FromBody]masterSpecialisationDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public masterSpecialisationDTO savedata([FromBody]masterSpecialisationDTO data)
        {
            return inter.savedata(data);
        }
        [Route("EditData")]
        public masterSpecialisationDTO EditData([FromBody]masterSpecialisationDTO data)
        {
            return inter.EditData(data);
        }
        [Route("masterDecative")]
        public masterSpecialisationDTO masterDecative([FromBody]masterSpecialisationDTO data)
        {
            return inter.masterDecative(data);
        }        
    }
}
