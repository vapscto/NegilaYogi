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
    public class INV_PurchaseIndentFacade : Controller
    {
        // GET: api/values
        INV_PurchaseIndentInterface _Inv;
        public INV_PurchaseIndentFacade(INV_PurchaseIndentInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_PurchaseIndentDTO getloaddata([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_PurchaseIndentDTO savedetails([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("getpidetails")]
        public Task<INV_PurchaseIndentDTO> getpidetails([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.getpidetails(data);
        }
        [Route("getprDetail")]
        public INV_PurchaseIndentDTO getprDetail([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.getprDetail(data);
        }
        [Route("deactiveM")]
        public INV_PurchaseIndentDTO deactiveM([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.deactiveM(data);
        }
        [Route("deactive")]
        public INV_PurchaseIndentDTO deactive([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.deactive(data);
        }

        [Route("get_details")]
        public INV_PurchaseIndentDTO get_details([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.get_details(data);
        }

        [Route("edit")]
        public INV_PurchaseIndentDTO edit([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.edit(data);
        }
        [Route("genrateReceipt")]
        public INV_PurchaseIndentDTO genrateReceipt([FromBody] INV_PurchaseIndentDTO data)
        {
            return _Inv.genrateReceipt(data);
        }









    }
}
