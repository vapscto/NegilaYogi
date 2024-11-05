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
    public class INV_MasterProductStagesFacadeController : Controller
    {
        // GET: api/values
        INV_MasterProductStagesInterface _Inv;
        public INV_MasterProductStagesFacadeController(INV_MasterProductStagesInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_Master_ProductDTO getloaddata([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public INV_Master_ProductDTO savedetails([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.savedetails(data);
        }
        [HttpPost]
        [Route("savedetailQty")]
        public INV_Master_ProductDTO savedetailQty([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.savedetailQty(data);
        }
        
        [Route("savestoreproduct")]
        public INV_Master_ProductDTO savestoreproduct([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.savestoreproduct(data);
        }

        [Route("deactive")]
        public INV_Master_ProductDTO deactive([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveQty")]
        public INV_Master_ProductDTO deactiveQty([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.deactiveQty(data);
        }
        [Route("deactiveptax")]
        public INV_Master_ProductDTO deactiveptax([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.deactiveptax(data);
        }

        
        [Route("productTax")]
        public INV_Master_ProductDTO productTax([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.productTax(data);
        }

        [Route("getstages")]
        public  INV_Master_ProductDTO getstages([FromBody] INV_Master_ProductDTO data)
        {
            return _Inv.getstages(data);
        }

    }
}
