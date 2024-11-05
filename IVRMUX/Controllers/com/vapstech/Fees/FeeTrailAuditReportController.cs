using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeTrailAuditReportController : Controller
    {

        FeeTrailAuditReportDelegate feeTrailAuditreport = new FeeTrailAuditReportDelegate();
       
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTrailAuditDTO Get(int id)
        {
            return feeTrailAuditreport.getdetails(id);
        }

        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public FeeTrailAuditDTO Get123(int id)
        {
            FeeTrailAuditDTO data = new FeeTrailAuditDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.useridpassing = UserId;
         
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmay_id = ASMAY_Id;

            return feeTrailAuditreport.getdata123(data);
        }



        // POST api/values
     
        [HttpPost]       
        [Route("getreport")]
        public FeeTrailAuditDTO getreport([FromBody] FeeTrailAuditDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data123.useridpassing = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data123.asmay_id = ASMAY_Id;

            return feeTrailAuditreport.getreport(data123);
        }


        [Route("viewdetails")]
        public FeeTrailAuditDTO viewdetails([FromBody] FeeTrailAuditDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data123.useridpassing = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data123.asmay_id = ASMAY_Id;

            return feeTrailAuditreport.viewdetails(data123);
        }



        [Route("searchfilter")]
        public FeeTrailAuditDTO searchfilter([FromBody] FeeTrailAuditDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data123.useridpassing = UserId;

            return feeTrailAuditreport.searchfilter(data123);
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
