using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServicesHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ReligionCategory_MappingFacade : Controller
    {
        // GET: api/<controller>
        public ReligionCategory_MappingInterface _rcc;
        public ReligionCategory_MappingFacade(ReligionCategory_MappingInterface admrcc)
        {
            _rcc = admrcc;
        }       
        [Route("loaddata")]
        public ReligionCategory_MappingDTO loaddata([FromBody] ReligionCategory_MappingDTO data)
        {
            return _rcc.loaddata(data);
        }
        [Route("savedata")]
        public ReligionCategory_MappingDTO savedata([FromBody] ReligionCategory_MappingDTO data)
        {
            return _rcc.savedata(data);
        }
        [Route("Editdata")]
        public ReligionCategory_MappingDTO Editdata([FromBody] ReligionCategory_MappingDTO data)
        {
            return _rcc.Editdata(data);
        }
        [Route("masterDecative")]
        public ReligionCategory_MappingDTO masterDecative([FromBody] ReligionCategory_MappingDTO data)
        {
            return _rcc.masterDecative(data);
        }
        //[Route("getcast")]
        //public ReligionCategory_MappingDTO getcast([FromBody] ReligionCategory_MappingDTO data)
        //{
        //    return _rcc.getcast(data);
        //}

    }
}
