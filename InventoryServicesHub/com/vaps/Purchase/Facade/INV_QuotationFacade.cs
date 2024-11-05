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
    public class INV_QuotationFacade : Controller
    {
        // GET: api/values
        INV_QuotationInterface _Inv;
        public INV_QuotationFacade(INV_QuotationInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_QuotationDTO getloaddata([FromBody] INV_QuotationDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getquotationdetails")]
        public INV_QuotationDTO getquotationdetails([FromBody] INV_QuotationDTO data)
        {
            return _Inv.getquotationdetails(data);
        }
        [Route("getpiDetail")]
        public INV_QuotationDTO getpiDetail([FromBody] INV_QuotationDTO data)
        {
            return _Inv.getpiDetail(data);
        }
        [Route("savedetails")]
        public INV_QuotationDTO savedetails([FromBody] INV_QuotationDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("deactiveM")]
        public INV_QuotationDTO deactiveM([FromBody] INV_QuotationDTO data)
        {
            return _Inv.deactiveM(data);
        }
        [Route("deactive")]
        public INV_QuotationDTO deactive([FromBody] INV_QuotationDTO data)
        {
            return _Inv.deactive(data);
        }









    }
}
