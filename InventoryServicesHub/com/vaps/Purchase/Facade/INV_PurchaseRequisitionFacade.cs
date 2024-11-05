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
    public class INV_PurchaseRequisitionFacade : Controller
    {
        // GET: api/values
        INV_PurchaseRequisitionInterface _Inv;
        public INV_PurchaseRequisitionFacade(INV_PurchaseRequisitionInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_PurchaseRequisitionDTO getloaddata([FromBody] INV_PurchaseRequisitionDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("get_prdetails")]
        public INV_PurchaseRequisitionDTO get_prdetails([FromBody] INV_PurchaseRequisitionDTO data)
        {
            return _Inv.get_prdetails(data);
        }
        [Route("getitemDetail")]
        public INV_PurchaseRequisitionDTO getitemDetail([FromBody] INV_PurchaseRequisitionDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public INV_PurchaseRequisitionDTO savedetails([FromBody] INV_PurchaseRequisitionDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("edit")]
        public INV_PurchaseRequisitionDTO edit([FromBody] INV_PurchaseRequisitionDTO data)
        {
            return _Inv.edit(data);
        }
        [Route("deactive")]
        public INV_PurchaseRequisitionDTO deactive([FromBody] INV_PurchaseRequisitionDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveM")]
        public INV_PurchaseRequisitionDTO deactiveM([FromBody] INV_PurchaseRequisitionDTO data)
        {
            return _Inv.deactiveM(data);
        }









    }
}
