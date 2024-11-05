using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Sales.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Sales.Facade
{
    [Route("api/[controller]")]
    public class ISM_ClientProject_MappingFacade : Controller
    {
        public ISM_ClientProject_MappingInterface inter;
        public ISM_ClientProject_MappingFacade(ISM_ClientProject_MappingInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public ISM_ClientProject_MappingDTO loaddata([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public ISM_ClientProject_MappingDTO savedata([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.savedata(data);
        }
        [Route("EditData")]
        public ISM_ClientProject_MappingDTO Editdata([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.Editdata(data);
        }
        [Route("getproject")]
        public ISM_ClientProject_MappingDTO getproject([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.getproject(data);
        }
        [Route("getmodule")]
        public ISM_ClientProject_MappingDTO getmodule([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.getmodule(data);
        }
        [Route("clientDecative")]
        public ISM_ClientProject_MappingDTO clientDecative([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.clientDecative(data);
        }
    }
}
