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
    public class monthendreportController : Controller
    {

        monthendreportDelegate feeTrailAuditreport = new monthendreportDelegate();
       
     
       
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public MonthEndReportDTO Get123(int id)
        {
            MonthEndReportDTO data = new MonthEndReportDTO();
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mi_id;
            return feeTrailAuditreport.getdata123(data);
        }



        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public MonthEndReportDTO getreport([FromBody] MonthEndReportDTO data123)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mi_id;
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
