using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Sales
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class Sales_Return_ApplyController : Controller
    {
        public Sales_Return_Apply_Delegate del = new Sales_Return_Apply_Delegate();

        [Route("getloaddata/{id:int}")]
        public Sales_Return_Apply_DTO getloaddata(int id)
        {
            Sales_Return_Apply_DTO data = new Sales_Return_Apply_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getloaddata(data);
        }
        [Route("getpidetails")]
        public Sales_Return_Apply_DTO getpidetails([FromBody] Sales_Return_Apply_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getpidetails(dto);
        }
        [Route("savedetails")]
        public Sales_Return_Apply_DTO savedetails([FromBody] Sales_Return_Apply_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }
    }
}