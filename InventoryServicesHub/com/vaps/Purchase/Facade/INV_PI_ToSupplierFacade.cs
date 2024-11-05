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
    public class INV_PI_ToSupplierFacade : Controller
    {
        // GET: api/values
        INV_PI_ToSupplierInterface _Inv;
        public INV_PI_ToSupplierFacade(INV_PI_ToSupplierInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_PI_ToSupplierDTO getloaddata([FromBody] INV_PI_ToSupplierDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getpiDetail")]
        public INV_PI_ToSupplierDTO getpiDetail([FromBody] INV_PI_ToSupplierDTO data)
        {
            return _Inv.getpiDetail(data);
        }
        [Route("savedetails")]
        public Task<INV_PI_ToSupplierDTO> savedetails([FromBody] INV_PI_ToSupplierDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("deactive")]
        public INV_PI_ToSupplierDTO deactive([FromBody] INV_PI_ToSupplierDTO data)
        {
            return _Inv.deactive(data);
        }









    }
}
