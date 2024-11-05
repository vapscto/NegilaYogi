using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.COE;
using PreadmissionDTOs.com.vaps.College.COE;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.COE
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgCOEReportController : Controller
    {
        ClgCOEReportDelegate objdelegate = new ClgCOEReportDelegate();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        //[HttpGet]
        //[Route("getalldetails/{id:int}")]
        //public ClgMasterCOEDTO getalldetails(int id)
        //{
        //    ClgMasterCOEDTO categorypage = new ClgMasterCOEDTO();
        //    categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
        //    categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return objdelegate.getdetails(categorypage);
        //}

        [HttpGet("{id:int}")]
        public ClgMasterCOEDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public ClgMasterCOEDTO getReport([FromBody]ClgMasterCOEDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getReport(data);
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

        [Route("mothreport")]
        public ClgMasterCOEDTO mothreport([FromBody]ClgMasterCOEDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return objdelegate.mothreport(data);
        }


    }
}
