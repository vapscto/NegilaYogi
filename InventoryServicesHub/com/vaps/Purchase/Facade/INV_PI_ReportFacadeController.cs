using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Purchase.Controllers
{
    [Route("api/[controller]")]
    public class INV_PI_ReportFacadeController : Controller
    {
        // GET: api/values
        INV_PI_ReportInterface _Inv;
        public INV_PI_ReportFacadeController(INV_PI_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_PurchaseIndentDTO> getloaddata([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_PurchaseIndentDTO> onreport([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
