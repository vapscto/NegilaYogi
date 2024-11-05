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
using IVRMUX.Delegates.com.vapstech.College.Fees;

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{

    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CollegeMothEndReportController :Controller
    {


        CollegeMothEndReportDelegate feeTrailAuditreport = new CollegeMothEndReportDelegate();



        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public FeeMonthEndReportDTO Get123(int id)
        {
            FeeMonthEndReportDTO data = new FeeMonthEndReportDTO();
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return feeTrailAuditreport.getdata123(data);
        }



        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public FeeMonthEndReportDTO getreport([FromBody] FeeMonthEndReportDTO data123)
        {
            data123.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data123.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
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
