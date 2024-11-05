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
    public class INV_ItemConsumptionFacadeController : Controller
    {
        // GET: api/values
        INV_ItemConsumptionInterface _Inv;
        public INV_ItemConsumptionFacadeController(INV_ItemConsumptionInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_ItemConsumptionDTO getloaddata([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.getloaddata(data);
        }


        [HttpPost]
        [Route("savedetails")]
        public INV_ItemConsumptionDTO savedetails([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("deactive")]
        public INV_ItemConsumptionDTO deactive([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveSub")]
        public INV_ItemConsumptionDTO deactiveSub([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.deactiveSub(data);
        }
        [Route("getobdetails")]
        public INV_ItemConsumptionDTO getobdetails([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.getobdetails(data);
        }

        [Route("getICDetails")]
        public Task<INV_ItemConsumptionDTO> getICDetails([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.getICDetails(data);
        }
        [Route("getsection")]
        public INV_ItemConsumptionDTO getsection([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.getsection(data);
        }
        [Route("getstudent")]
        public INV_ItemConsumptionDTO getstudent([FromBody] INV_ItemConsumptionDTO data)
        {
            return _Inv.getstudent(data);
        }



    }
}
