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
    public class INV_PhyStock_UpdationFacade : Controller
    {
        // GET: api/values
        INV_PhyStock_UpdationInterface _Inv;
        public INV_PhyStock_UpdationFacade(INV_PhyStock_UpdationInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_PhyStock_UpdationDTO getloaddata([FromBody] INV_PhyStock_UpdationDTO data)
        {
            return _Inv.getloaddata(data);
        }
      

        [HttpPost]
        [Route("savedetails")]
        public INV_PhyStock_UpdationDTO savedetails([FromBody] INV_PhyStock_UpdationDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("deactive")]
        public INV_PhyStock_UpdationDTO deactive([FromBody] INV_PhyStock_UpdationDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("getobdetails")]
        public INV_PhyStock_UpdationDTO getobdetails([FromBody] INV_PhyStock_UpdationDTO data)
        {
            return _Inv.getobdetails(data);
        }


        
    }
}
