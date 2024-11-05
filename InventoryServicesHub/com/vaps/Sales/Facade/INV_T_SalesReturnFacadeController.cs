using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Sales.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace InventoryServicesHub.com.vaps.Sales.Facade
{
 
    [Route("api/[controller]")]
    public class INV_T_SalesReturnFacadeController : Controller
    {
        INV_T_SalesReturnInterface _Inv;
        public INV_T_SalesReturnFacadeController(INV_T_SalesReturnInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_T_SalesReturnDTO> getloaddata([FromBody] INV_T_SalesReturnDTO data)
        {
            return _Inv.getloaddata(data);
        }
       
     
        [Route("getitem")]
        public Task<INV_T_SalesReturnDTO> getitem([FromBody] INV_T_SalesReturnDTO data)
        {
            return _Inv.getitem(data);
        }
        [Route("getitemDetail")]
        public Task<INV_T_SalesReturnDTO> getitemDetail([FromBody] INV_T_SalesReturnDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public Task<INV_T_SalesReturnDTO> savedetails([FromBody] INV_T_SalesReturnDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("deactive")]
        public Task<INV_T_SalesReturnDTO> deactive([FromBody] INV_T_SalesReturnDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("viewitem")]
        public Task<INV_T_SalesReturnDTO> viewitem([FromBody] INV_T_SalesReturnDTO data)
        {
            return _Inv.viewitem(data);
        }

    }
}