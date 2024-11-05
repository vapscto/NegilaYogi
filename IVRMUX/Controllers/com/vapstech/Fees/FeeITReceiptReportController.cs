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
    public class FeeITReceiptReportController : Controller
    {

        FeeITReceiptReportDelegate feeTrailAuditreport = new FeeITReceiptReportDelegate();
       
       
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public FeeITReceiptDTO Get123(int id)
        {
            FeeITReceiptDTO data = new FeeITReceiptDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmyid = ASMAY_Id;

            return feeTrailAuditreport.getdata123(data);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public FeeITReceiptDTO getstuddetails([FromBody]FeeITReceiptDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.asmyid = ASMAY_Id;

            return feeTrailAuditreport.getstuddet(value);
        }

        // POST api/values

        [HttpPost]       
        [Route("getreport")]
        public FeeITReceiptDTO getreport([FromBody] FeeITReceiptDTO data123)
        {
           int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;

          //  int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
         //   data123.asmyid = ASMAY_Id;

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

        [HttpPost]
        [Route("selectacademicyear")]
        public FeeITReceiptDTO selectacademicyear([FromBody] FeeITReceiptDTO pgmodu)
        {
            pgmodu.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //pgmodu.asmyid = ASMAY_Id;
            return feeTrailAuditreport.selectacademicyear(pgmodu);
        }

    }
}
