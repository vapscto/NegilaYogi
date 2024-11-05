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
    public class INV_MasterItemFacadeController : Controller
    {
        // GET: api/values
        INV_MasterItemInterface _Inv;
        public INV_MasterItemFacadeController(INV_MasterItemInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_Master_ItemDTO getloaddata([FromBody] INV_Master_ItemDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public INV_Master_ItemDTO savedetails([FromBody] INV_Master_ItemDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("deactive")]
        public INV_Master_ItemDTO deactive([FromBody] INV_Master_ItemDTO data)
        {
            return _Inv.deactive(data);
        }

        [Route("deactiveitax")]
        public INV_Master_ItemDTO deactiveitax([FromBody] INV_Master_ItemDTO data)
        {
            return _Inv.deactiveitax(data);
        }

        
        [Route("itemTax")]
        public INV_Master_ItemDTO itemTax([FromBody] INV_Master_ItemDTO data)
        {
            return _Inv.itemTax(data);
        }



    }
}
