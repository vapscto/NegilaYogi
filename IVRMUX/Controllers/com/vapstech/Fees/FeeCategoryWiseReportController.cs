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
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeCategoryWiseReportController : Controller
    {

        FeeCategoryWiseReportDelegate feeTrailAuditreport = new FeeCategoryWiseReportDelegate();
           
       
      
        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public FeeCategoryWiseReportDTO getreport([FromBody] FeeCategoryWiseReportDTO data123)
        {
            data123.yearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data123.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return feeTrailAuditreport.getreport(data123);
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
