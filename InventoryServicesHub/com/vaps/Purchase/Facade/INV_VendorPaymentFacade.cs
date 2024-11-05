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
    public class INV_VendorPaymentFacade : Controller
    {
        // GET: api/values
        INV_VendorPaymentInterface _Inv;
        public INV_VendorPaymentFacade(INV_VendorPaymentInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_VendorPaymentDTO getloaddata([FromBody] INV_VendorPaymentDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getGRNdetail")]
        public Task<INV_VendorPaymentDTO> getGRNdetail([FromBody] INV_VendorPaymentDTO data)
        {
            return _Inv.getGRNdetail(data);
        }
        [Route("savedetails")]
        public INV_VendorPaymentDTO savedetails([FromBody] INV_VendorPaymentDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("deactive")]
        public INV_VendorPaymentDTO deactive([FromBody] INV_VendorPaymentDTO data)
        {
            return _Inv.deactive(data);
        }
        
        [Route("deactiveGRN")]
        public INV_VendorPaymentDTO deactiveGRN([FromBody] INV_VendorPaymentDTO data)
        {
            return _Inv.deactiveGRN(data);
        }
        
        [Route("getmodeldetail")]
        public INV_VendorPaymentDTO getmodeldetail([FromBody] INV_VendorPaymentDTO data)
        {
            return _Inv.getmodeldetail(data);
        }









    }
}
