

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterSubjectGroupController : Controller
    {
        MasterSubjectGroupDelegates objdelegate = new MasterSubjectGroupDelegates();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public MasterSubjectGroupDTO Get([FromQuery] int id)
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
        public MasterSubjectGroupDTO savedetail([FromBody] MasterSubjectGroupDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            //categorypage.EMGR_MarksPerFlag = Convert.ToChar(categorypage.EMGR_MarksPerFlag);
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(categorypage);
        }


        [HttpPost]
        [Route("deactivate")]
        public MasterSubjectGroupDTO deactivate([FromBody] MasterSubjectGroupDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate(categorypage);
        }


        [Route("getalldetailsviewrecords/{id:int}")]
        public MasterSubjectGroupDTO getalldetailsviewrecords(int id)
        {

            return objdelegate.getalldetailsviewrecords(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterSubjectGroupDTO getdetail(int id)
        {
            return objdelegate.getpagedetails(id);

        }

        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public MasterSubjectGroupDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
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
