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
    public class INV_PurchaseOrderFacade : Controller
    {
        // GET: api/values
        INV_PurchaseOrderInterface _Inv;
        public INV_PurchaseOrderFacade(INV_PurchaseOrderInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_PurchaseOrderDTO getloaddata([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getqtDetail")]
        public INV_PurchaseOrderDTO getqtDetail([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.getqtDetail(data);
        }
        [Route("getitemtax")]
        public INV_PurchaseOrderDTO getitemtax([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.getitemtax(data);
        }
        
        [Route("savedetails")]
        public INV_PurchaseOrderDTO savedetails([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("deactiveM")]
        public INV_PurchaseOrderDTO deactiveM([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.deactiveM(data);
        }
        [Route("deactive")]
        public INV_PurchaseOrderDTO deactive([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveTx")]
        public INV_PurchaseOrderDTO deactiveTx([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.deactiveTx(data);
        }
        [Route("get_modeldetails")]
        public INV_PurchaseOrderDTO get_modeldetails([FromBody] INV_PurchaseOrderDTO data)
        {
            return _Inv.get_modeldetails(data);
        }


        





    }
}
