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
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class Sales_Return_ApprovalFacadeController : Controller
    {
        Sales_Return_Approval_Interface _inter;
        public Sales_Return_ApprovalFacadeController(Sales_Return_Approval_Interface inter)
        {
            _inter = inter;
        }
        [Route("getloaddata")]
        public Sales_Return_Apply_DTO getloaddata([FromBody] Sales_Return_Apply_DTO dto)
        {
            return _inter.getloaddata(dto);
        }
        [Route("getpidetails")]
        public Task<Sales_Return_Apply_DTO> getpidetails([FromBody] Sales_Return_Apply_DTO dto)
        {
            return _inter.getpidetails(dto);
        }
        [Route("savedetails")]
        public Sales_Return_Apply_DTO savedetails([FromBody] Sales_Return_Apply_DTO dto)
        {
            return _inter.savedetails(dto);
        }

    }
}