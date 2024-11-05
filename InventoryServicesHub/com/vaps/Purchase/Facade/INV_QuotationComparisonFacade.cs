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
    public class INV_QuotationComparisonFacade : Controller
    {
        // GET: api/values
        INV_QuotationComparisonInterface _Inv;
        public INV_QuotationComparisonFacade(INV_QuotationComparisonInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_QuotationDTO getloaddata([FromBody] INV_QuotationDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getpisupplier")]
        public INV_QuotationDTO getpisupplier([FromBody] INV_QuotationDTO data)
        {
            return _Inv.getpisupplier(data);
        }
        [Route("get_Comparison")]
        public INV_QuotationDTO get_Comparison([FromBody] INV_QuotationDTO data)
        {
            return _Inv.get_Comparison(data);
        }
        [Route("getqtdetails")]
        public INV_QuotationDTO getqtdetails([FromBody] INV_QuotationDTO data)
        {
            return _Inv.getqtdetails(data);
        }
        [Route("savedata")]
        public INV_QuotationDTO savedata([FromBody] INV_QuotationDTO data)
        {
            return _Inv.savedata(data);
        }

        




    }
}
