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
    public class FeeConsolidatedReportController : Controller
    {

        FeeConsolidatedReportDelegate _feeSummary = new FeeConsolidatedReportDelegate();
       
     
       
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public FeeConsolidatedReportDTO Get123(int id)
        {
            FeeConsolidatedReportDTO data = new FeeConsolidatedReportDTO();
            // data.MI_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _feeSummary.getdata123(data);
        }



        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public FeeConsolidatedReportDTO getreport([FromBody] FeeConsolidatedReportDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_Id = mid;

            return _feeSummary.getreport(data123);
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
