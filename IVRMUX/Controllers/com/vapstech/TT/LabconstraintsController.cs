using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.TT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class LabconstraintsController : Controller
    {
        LabconstraintsDelegate objdelegate = new LabconstraintsDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TT_LABLIB_DTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public TT_LABLIB_DTO savedetail([FromBody] TT_LABLIB_DTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(categorypage);
        }
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public TT_LABLIB_DTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }
      
        [Route("getdetails/{id:int}")]
        public TT_LABLIB_DTO getdetail(int id)
        {
            return objdelegate.getpagedetails(id);

        }       
        [Route("getalldetailsviewrecords/{id:int}")]
        public TT_LABLIB_DTO getalldetailsviewrecords(int id)
        {

            return objdelegate.getalldetailsviewrecords(id);
        }
        //for get class
        [Route("getclass_catg")]
        public TT_LABLIB_DTO getclass_catg([FromBody] TT_LABLIB_DTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclass_catg(categorypage);
        }
        [Route("get_catg")]
        public TT_LABLIB_DTO get_catg([FromBody] TT_LABLIB_DTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_catg(categorypage);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("deactivate")]
        public TT_LABLIB_DTO deactvate([FromBody] TT_LABLIB_DTO id)
        {
            return objdelegate.deactivate(id);
        }

    }
}
