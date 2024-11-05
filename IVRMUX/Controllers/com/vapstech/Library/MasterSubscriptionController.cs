using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class MasterSubscriptionController : Controller
    {

        MasterSubscriptionDelegate _objdel = new MasterSubscriptionDelegate();

        [Route("getdetails/{id:int}")]
        public Master_Subscription_DTO getdetails(int id)
        {
            Master_Subscription_DTO dto = new Master_Subscription_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getdetails(dto);
        }

        [Route("EditData")]
        public Master_Subscription_DTO EditData([FromBody]Master_Subscription_DTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.EditData(dto);
        }

        [HttpPost]
        [Route("Savedata")]
        public Master_Subscription_DTO Savedata([FromBody]Master_Subscription_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _objdel.Savedata(data);
        }

        [Route("deactiveY")]
        public Master_Subscription_DTO deactiveY([FromBody] Master_Subscription_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deactiveY(d);
        }
    }
}
