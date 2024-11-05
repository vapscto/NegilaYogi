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
    public class DCS_Vendor_PaymentFacadeController : Controller
    {
        // GET: api/values
        DCS_Vendor_PaymentInterface _Inv;
        public DCS_Vendor_PaymentFacadeController(DCS_Vendor_PaymentInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_T_SalesDTO> getloaddata([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.getloaddata(data);
        }
      
        [Route("getitem")]
        public INV_T_SalesDTO getitem([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.getitem(data);
        }
        [Route("getitemDetail")]
        public Task<INV_T_SalesDTO> getitemDetail([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public INV_T_SalesDTO savedetails([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("getSaletypes")]
        public Task<INV_T_SalesDTO> getSaletypes([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.getSaletypes(data);
        }
        [Route("getbilldetails")]
        public Task<INV_T_SalesDTO> getbilldetails([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.getbilldetails(data);
        }
        [Route("getSaleItemTax")]
        public INV_T_SalesDTO getSaleItemTax([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.getSaleItemTax(data);
        }
        [Route("deactive")]
        public INV_T_SalesDTO deactive([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveS")]
        public INV_T_SalesDTO deactiveS([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.deactiveS(data);
        }
        [Route("deactivetax")]
        public INV_T_SalesDTO deactivetax([FromBody] INV_T_SalesDTO data)
        {
            return _Inv.deactivetax(data);
        }


    }
}
