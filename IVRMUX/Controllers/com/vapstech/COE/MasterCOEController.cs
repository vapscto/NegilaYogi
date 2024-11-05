using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.COE
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterCOEController : Controller
    {
        MasterCOEDelegate objdelegate = new MasterCOEDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public MasterCOEDTO Get([FromQuery] int id)

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
        [Route("savedetail1")]
        public MasterCOEDTO savedetail1([FromBody] MasterCOEDTO categorypage)
        {
          //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail1(categorypage);
        }
        [HttpPost]
        [Route("savedetail2")]
        public MasterCOEDTO savedetail2([FromBody] MasterCOEDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail2(categorypage);
        }

        [HttpPost]
        [Route("deactivate1")]
        public MasterCOEDTO deactivate1([FromBody] MasterCOEDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate1(categorypage);
        }
        [HttpPost]
        [Route("deactivate2")]
        public MasterCOEDTO deactivate2([FromBody] MasterCOEDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate2(categorypage);
        }

        [Route("geteventdetails")]
        public MasterCOEDTO geteventdetails([FromBody] MasterCOEDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.geteventdetails(categorypage);

        }
        [Route("getalldetailsviewrecords1/{id:int}")]
        public MasterCOEDTO getalldetailsviewrecords1(int id)
        {

            return objdelegate.getalldetailsviewrecords1(id);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        public MasterCOEDTO getalldetailsviewrecords2(int id)
        {

            return objdelegate.getalldetailsviewrecords2(id);
        }
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public MasterCOEDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }
        [Route("getdetails1/{id:int}")]
        public MasterCOEDTO getdetail1(int id)
        {
            return objdelegate.getpagedetails1(id);

        }
        [Route("getdetails2/{id:int}")]
        public MasterCOEDTO getdetail2(int id)
        {
            return objdelegate.getpagedetails2(id);

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


    }
}
