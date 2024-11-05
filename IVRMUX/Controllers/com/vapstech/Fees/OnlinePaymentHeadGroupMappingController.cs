using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class OnlinePaymentHeadGroupMappingController : Controller
    {
        OnlinePaymentHeadGroupMappingDelegate gate = new OnlinePaymentHeadGroupMappingDelegate();
        // GET: api/values

        [Route("onlineMappingDetails/{id:int}")]
        public Fee_OnlinePayment_MappingDTO onlineMappingDetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return gate.getInitailData(id);
        }

        // POST api/values
        [HttpPost]
        public Fee_OnlinePayment_MappingDTO save([FromBody] Fee_OnlinePayment_MappingDTO data)
        {
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.savedata(data);
        }
        [Route("edit/{id:int}")]
        public Fee_OnlinePayment_MappingDTO editdata(int id)
        {
            return gate.editdata(id);
        }
        [Route("delete/{id:int}")]
        public Fee_OnlinePayment_MappingDTO deletedata(int id)
        {
            return gate.deletedata(id);
        }

        [Route("groupsel")]
        public Fee_OnlinePayment_MappingDTO groupselection([FromBody] Fee_OnlinePayment_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return gate.selecgroup(data);
        }

        [Route("headsel")]
        public Fee_OnlinePayment_MappingDTO headselection([FromBody] Fee_OnlinePayment_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return gate.selechead(data);
        }

        [Route("academicsel")]
        public Fee_OnlinePayment_MappingDTO acdesel([FromBody] Fee_OnlinePayment_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return gate.acdesel(data);
        }

    }
}
