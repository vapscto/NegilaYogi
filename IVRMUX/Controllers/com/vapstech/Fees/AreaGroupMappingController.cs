using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.Transport;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AreaGroupMappingController : Controller
    {        
        AreaGroupMappingDelegate feeHd = new AreaGroupMappingDelegate();

        //loaddata
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public AreaGroupMappingDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return feeHd.getdetails(id);
        }
        
        //for edit
        [Route("getdetails/{id:int}")]
        public AreaGroupMappingDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            return feeHd.getpagedetails(id);

        }
       
        // POST api/values

        [HttpPost]
        public AreaGroupMappingDTO savedetail([FromBody] AreaGroupMappingDTO data)
        {
            data.MI_Id =  Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return feeHd.savedetails(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public AreaGroupMappingDTO deactvate([FromBody] AreaGroupMappingDTO id)
        {
            return feeHd.deactivate(id);
        }

        [Route("savedataamount")]
        public TR_Area_AmountDTO savedataamount([FromBody] TR_Area_AmountDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("User_Id"));
            return feeHd.savedataamount(data);
        }

        [Route("geteditdataamount")]
        public TR_Area_AmountDTO geteditdataamount([FromBody]TR_Area_AmountDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("User_Id"));
            return feeHd.geteditdataamount(data);
        }

        [Route("activedeactiveamount")]
        public TR_Area_AmountDTO activedeactiveamount([FromBody] TR_Area_AmountDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("User_Id"));
            return feeHd.activedeactiveamount(data);
        }

    }
}
