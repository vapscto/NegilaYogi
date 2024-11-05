using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class SpecialFeeHeadClgController : Controller
    {
        SpecialFeeHeadClgDelegate fshd = new SpecialFeeHeadClgDelegate();


        [HttpPost]
        [Route("savedetailY")]
        public SpecialFeeHeadClgDTO savedetailY([FromBody] SpecialFeeHeadClgDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fshd.savedetailsY(GrouppageY);
        }

        //    GET api/values/5
        [HttpGet]
        [Route("getalldetailsY/{id:int}")]
        public SpecialFeeHeadClgDTO getalldetailsY([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fshd.getdetailsY(id);
        }


        [HttpPost]
        [Route("deactivateY")]
        public SpecialFeeHeadClgDTO deactvateY([FromBody] SpecialFeeHeadClgDTO id)
        {
            return fshd.deactivateY(id);
        }


        [Route("getdetailsY/{id:int}")]
        public SpecialFeeHeadClgDTO getdetailY(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return fshd.getpagedetailsY(id);

        }


        // DELETE api/values/5
        [HttpPost]
        [Route("deletepagesY/{id:int}")]
        public SpecialFeeHeadClgDTO DeleteY(int id)
        {
            SpecialFeeHeadClgDTO data = new SpecialFeeHeadClgDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.FMSFHFH_Id = id;
            return fshd.deleterecY(data);
        }

    }
}
