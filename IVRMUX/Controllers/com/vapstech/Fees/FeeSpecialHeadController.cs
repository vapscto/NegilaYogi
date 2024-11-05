using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeSpecialHeadController : Controller
    {
        FeeSpecialHeadDelegate fshd = new FeeSpecialHeadDelegate();

        
        [HttpPost]
        [Route("savedetailY")]
        public FeeSpecialFeeGroupDTO savedetailY([FromBody] FeeSpecialFeeGroupDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fshd.savedetailsY(GrouppageY);
        }

        //    GET api/values/5
        [HttpGet]
        [Route("getalldetailsY/{id:int}")]
        public FeeSpecialFeeGroupDTO getalldetailsY([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fshd.getdetailsY(id);
        }


        [HttpPost]
        [Route("deactivateY")]
        public FeeSpecialFeeGroupDTO deactvateY([FromBody] FeeSpecialFeeGroupDTO id)
        {
            return fshd.deactivateY(id);
        }


        [Route("getdetailsY/{id:int}")]
        public FeeSpecialFeeGroupDTO getdetailY(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return fshd.getpagedetailsY(id);

        }


        // DELETE api/values/5
        [HttpPost]
        [Route("deletepagesY/{id:int}")]
        public FeeSpecialFeeGroupDTO DeleteY(int id)
        {
            FeeSpecialFeeGroupDTO data = new FeeSpecialFeeGroupDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.FMSFHFH_Id = id;
            return fshd.deleterecY(data);
        }
    }
}
