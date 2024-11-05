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
    public class INV_OpeningBalanceFacadeController : Controller
    {
        // GET: api/values
        INV_OpeningBalanceInterface _Inv;
        public INV_OpeningBalanceFacadeController(INV_OpeningBalanceInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_OpeningBalanceDTO getloaddata([FromBody] INV_OpeningBalanceDTO data)
        {
            return _Inv.getloaddata(data);
        }
      

        [HttpPost]
        [Route("savedetails")]
        public INV_OpeningBalanceDTO savedetails([FromBody] INV_OpeningBalanceDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("deactive")]
        public INV_OpeningBalanceDTO deactive([FromBody] INV_OpeningBalanceDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("getobdetails")]
        public INV_OpeningBalanceDTO getobdetails([FromBody] INV_OpeningBalanceDTO data)
        {
            return _Inv.getobdetails(data);
        }
        [Route("move_to_stock")]
        public INV_OpeningBalanceDTO move_to_stock([FromBody] INV_OpeningBalanceDTO data)
        {
            return _Inv.move_to_stock(data);
        }


        
    }
}
