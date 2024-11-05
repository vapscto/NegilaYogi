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
    public class PeriodAllocationController : Controller
    {
        PeriodAllocationDelegate objdelegate = new PeriodAllocationDelegate();
        
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TTPeriodAllocationDTO Get([FromQuery] int id)
        {
            TTPeriodAllocationDTO data = new TTPeriodAllocationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate.getdetails(data);
        }
        // POST api/values
        [HttpPost]
        [Route("getclass_catg")]
        public TTPeriodAllocationDTO getclasses([FromBody] TTPeriodAllocationDTO data)
        {
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclasses(data);
        }
        [HttpPost]
        [Route("get_catg")]
        public TTPeriodAllocationDTO getcategories([FromBody] TTPeriodAllocationDTO data)
        {
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getcategories(data);
        }
        [HttpPost]
        [Route("getperiod_class")]
        public TTPeriodAllocationDTO getperiod_class([FromBody] TTPeriodAllocationDTO data)
        {
           // data.ASAMY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getperiod_class(data);
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPost]
        [Route("saveperiod")]
        public TTPeriodAllocationDTO saveperiod([FromBody] TTPeriodAllocationDTO periodpage)
        {
            // periodpage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.saveperiod(periodpage);
        }

        [HttpPost]
        [Route("savedetail")]
        public TTPeriodAllocationDTO savedetail([FromBody] TTPeriodAllocationDTO periodpage)
        {
           // periodpage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(periodpage);
        }
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public TTPeriodAllocationDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }

        [Route("getdetails/{id:int}")]
        public TTPeriodAllocationDTO getdetail(int id)
        {
            return objdelegate.getpagedetails(id);

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
        public TTPeriodAllocationDTO deactvate([FromBody] TTPeriodAllocationDTO id)
        {
            return objdelegate.deactivate(id);
        }
        [HttpPost]
        [Route("deactivate1")]
        public TTPeriodAllocationDTO deactvate1([FromBody] TTPeriodAllocationDTO id)
        {
            return objdelegate.deactivate1(id);
        }


    }
}
