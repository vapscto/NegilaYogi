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
    public class INV_MasterStoreFacadeController : Controller
    {
        // GET: api/values
        INV_MasterStoreInterface _Inv;
        public INV_MasterStoreFacadeController(INV_MasterStoreInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_Master_StoreDTO getloaddata([FromBody] INV_Master_StoreDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public INV_Master_StoreDTO savedetails([FromBody] INV_Master_StoreDTO data)
        {
            return _Inv.savedetails(data);
        }
       
        [Route("deactive")]
        public INV_Master_StoreDTO deactive([FromBody] INV_Master_StoreDTO data)
        {
            return _Inv.deactive(data);
        }
        


    }
}
