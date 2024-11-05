using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class INV_MasterProductStagesController : Controller
    {
        INV_MasterProductStagesDelegate _delegate = new INV_MasterProductStagesDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_Master_ProductDTO getloaddata(int id)
        {
            INV_Master_ProductDTO data = new INV_Master_ProductDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_Master_ProductDTO savedetails([FromBody] INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetails(data);
        }
        [Route("savedetailQty")]
        public INV_Master_ProductDTO savedetailQty([FromBody] INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetailQty(data);
        }

        [Route("savestoreproduct")]
        public INV_Master_ProductDTO savestoreproduct([FromBody] INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savestoreproduct(data);
        }

        [Route("deactive")]
        public INV_Master_ProductDTO deactive([FromBody] INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveQty")]
        public INV_Master_ProductDTO deactiveQty([FromBody] INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveQty(data);
        }
    
         [Route("deactiveptax")]
        public INV_Master_ProductDTO deactiveptax([FromBody] INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveptax(data);
        }

        [Route("productTax")]
        public INV_Master_ProductDTO productTax([FromBody] INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.productTax(data);
        }
        
        [Route("getstages")]
        public INV_Master_ProductDTO getstages([FromBody] INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getstages(data);
        }



    }
}
