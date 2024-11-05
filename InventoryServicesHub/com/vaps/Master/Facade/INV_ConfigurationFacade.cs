using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class INV_ConfigurationFacade : Controller
    {
        // GET: api/values
        INV_ConfigurationInterface _Inv;
        public INV_ConfigurationFacade(INV_ConfigurationInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_ConfigurationDTO> getloaddata([FromBody] INV_ConfigurationDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public INV_ConfigurationDTO savedetails([FromBody] INV_ConfigurationDTO data)
        {
            return _Inv.savedetails(data);
        }

    }
}
