using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Sales.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Sales.Controllers
{
    [Route("api/[controller]")]
    public class INV_ItemConsumptionReportFacade : Controller
    {
        // GET: api/values
        INV_ItemConsumptionReportInterface _Inv;
        public INV_ItemConsumptionReportFacade(INV_ItemConsumptionReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_ItemConsumptionDTO> getloaddata([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_ItemConsumptionDTO> onreport([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
