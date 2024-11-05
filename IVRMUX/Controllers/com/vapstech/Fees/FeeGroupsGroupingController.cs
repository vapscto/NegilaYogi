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
    public class FeeGroupsGroupingController : Controller
    {
        FeeGroupGroupingDelegate FGGD = new FeeGroupGroupingDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/values
        [HttpPost]
        [Route("savedetailY")]
        public FeeGroupMappingDTO savedetailY([FromBody] FeeGroupMappingDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGGD.savedetailsY(GrouppageY);
        }

        //    GET api/values/5
        [HttpGet]
        [Route("getalldetailsY/{id:int}")]
        public FeeGroupMappingDTO getalldetailsY([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGGD.getdetailsY(id);
        }


        [HttpPost]
        [Route("deactivateY")]
        public FeeGroupMappingDTO deactvateY([FromBody] FeeGroupMappingDTO id)
        {
            return FGGD.deactivateY(id);
        }


        [Route("getdetailsY/{id:int}")]
        public FeeGroupMappingDTO getdetailY(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGGD.getpagedetailsY(id);

        }


        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepagesY/{id:int}")]
        public FeeGroupMappingDTO DeleteY(int id)
        {
            return FGGD.deleterecY(id);
        }

    }
}
