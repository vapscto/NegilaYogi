using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Masters
{

    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeHeadClgController : Controller
    {
        // GET: api/values
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

        FeeHeadClgDelegate feeHd = new FeeHeadClgDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeHeadClgDTO Get( int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return feeHd.getdetails(id);
        }
        //for edit

        [Route("getdetails/{id:int}")]
        public FeeHeadClgDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return feeHd.getpagedetails(id);
        }
        [Route("Editdetails/{id:int}")]
        public FeeHeadClgDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return feeHd.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public FeeHeadClgDTO savedetail([FromBody] FeeHeadClgDTO GroupHeadpage)
        {
            GroupHeadpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
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
        public FeeHeadClgDTO Delete(int id)
        {
            return feeHd.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeHeadClgDTO deactvate([FromBody] FeeHeadClgDTO id)
        {
            return feeHd.deactivateAcademicYear(id);
        }

        [Route("validateordernumber")]
        public FeeHeadClgDTO validateordernumber([FromBody] FeeHeadClgDTO GroupOrder)
        {
            return feeHd.validateordernumber(GroupOrder);
        }

        [Route("getallbankdetails")]
        public FeeHeadClgDTO getallbankdetails([FromBody] FeeHeadClgDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return feeHd.getallbankdetails(data);
        }

        [Route("savedata")]
        public FeeHeadClgDTO savedata([FromBody] FeeHeadClgDTO GroupHeadpage)
        {
            GroupHeadpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GroupHeadpage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeHd.savedata(GroupHeadpage);
        }
        [Route("edit")]
        public FeeHeadClgDTO edit([FromBody] FeeHeadClgDTO GroupHeadpage)
        {
            GroupHeadpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GroupHeadpage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeHd.edit(GroupHeadpage);
        }
        [Route("activedeactive")]
        public FeeHeadClgDTO activedeactive([FromBody] FeeHeadClgDTO GroupHeadpage)
        {
            GroupHeadpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GroupHeadpage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeHd.activedeactive(GroupHeadpage);
        }
    }
}
