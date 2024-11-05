using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CLGRegNoFormatController : Controller
    {
        CLGRegNoFormatDelegate objDel =new CLGRegNoFormatDelegate();
        
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
        [HttpPost]
        [Route("Savedetails")]
        public CLGAdm_College_RegNo_FormatDTO Savedetails([FromBody]CLGAdm_College_RegNo_FormatDTO id)
        {
            //id.MI_Id = 4;
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Savedetails(id);
        }
        [HttpGet]
        [Route("getalldetails")]
        public CLGAdm_College_RegNo_FormatDTO getalldetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.getalldetails(id);
        }
        [HttpPost]
        [Route("Deletedetails")]
        public CLGAdm_College_RegNo_FormatDTO Deletedetails([FromBody]CLGAdm_College_RegNo_FormatDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objDel.Deletedetails(id);
        }
    }
}
