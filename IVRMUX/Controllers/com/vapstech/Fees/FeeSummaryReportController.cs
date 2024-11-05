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
    public class FeeSummaryReportController : Controller
    {

        FeeSummaryReportDelegate _feeSummary = new FeeSummaryReportDelegate();
       
     
       
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public FeeSummaryReportDTO Get123(int id)
        {
            FeeSummaryReportDTO data = new FeeSummaryReportDTO();
            //data.MI_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _feeSummary.getdata123(data);
        }



        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public FeeSummaryReportDTO getreport([FromBody] FeeSummaryReportDTO data123)
        {
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
