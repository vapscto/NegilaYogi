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
    public class FeeHeadController : Controller
    {
        //FeeHeadDelegate feeHd = new FeeHeadDelegate();
        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        FeeHeadDelegate feeHd = new FeeHeadDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeHeadDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return feeHd.getdetails(id);
        }
        //for edit
        [Route("getdetails/{id:int}")]
        public FeeHeadDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return feeHd.getpagedetails(id);
        }
        [Route("Editdetails/{id:int}")]
        public FeeHeadDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return feeHd.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public FeeHeadDTO savedetail([FromBody] FeeHeadDTO GroupHeadpage)
        {
            GroupHeadpage.MI_Id =  Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GroupHeadpage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeHd.savedetails(GroupHeadpage);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public FeeHeadDTO Delete(int id)
        {
            return feeHd.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeHeadDTO deactvate([FromBody] FeeHeadDTO id)
        {
            return feeHd.deactivateAcademicYear(id);
        }

        [Route("validateordernumber")]
        public FeeHeadDTO validateordernumber([FromBody] FeeHeadDTO GroupOrder)
        {
            return feeHd.validateordernumber(GroupOrder);
        }
    }
}
