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
    public class INV_VendorPayment_ReportFacade : Controller
    {
        // GET: api/values
        INV_VendorPayment_ReportInterface _Inv;
        public INV_VendorPayment_ReportFacade(INV_VendorPayment_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_VendorPaymentDTO> getloaddata([FromBody] INV_VendorPaymentDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_VendorPaymentDTO> onreport([FromBody] INV_VendorPaymentDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
