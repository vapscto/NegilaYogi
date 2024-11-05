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
    public class INV_PO_ReportFacadeController : Controller
    {
        // GET: api/values
        INV_PO_ReportInterface _Inv;
        public INV_PO_ReportFacadeController(INV_PO_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_PurchaseOrderDTO> getloaddata([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_PurchaseOrderDTO> onreport([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
