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
    public class ClassMasterController : Controller
    {
        ClassMasterDelegate objdelegate = new ClassMasterDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public TTClassMasterDTO Get([FromQuery] int id)
        {
            TTClassMasterDTO data = new TTClassMasterDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return objdelegate.getdetails(data);
        }
        [Route("getalldetailsviewrecords/{id:int}")]
        public TTClassMasterDTO getalldetailsviewrecords(int id)
        {

            return objdelegate.getalldetailsviewrecords(id);
        }

        [HttpPost]
        [Route("get_catg")]
        public TTClassMasterDTO getcategories([FromBody] TTClassMasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getcategories(data);
        }

        // POST api/values
        [HttpPost]
        [Route("getclass_catg")]
        public TTClassMasterDTO getclasses([FromBody] TTClassMasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getclasses(data);
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public TTClassMasterDTO savedetail([FromBody] TTClassMasterDTO periodpage)
        {
           
            periodpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(periodpage);
        }
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public TTClassMasterDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public TTClassMasterDTO deactivate([FromBody] TTClassMasterDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate(categorypage);
        }
        [Route("getdetails/{id:int}")]
        public TTClassMasterDTO getdetail(int id)
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



    }
}
