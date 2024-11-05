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
    public class FeeReceiptReportController : Controller
    {

        FeeReceiptReportDelegate feeTrailAuditreport = new FeeReceiptReportDelegate();
       
     
       
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public FeeReceiptDTO Get123(int id)
        {
            FeeReceiptDTO data = new FeeReceiptDTO();
            //data.MI_ID = id;
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.acayyearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return feeTrailAuditreport.getdata123(data);
        }
        [Route("getinsdetils/{id:int}")]
        public FeeReceiptDTO getinsdetils(int id)
        {
            FeeReceiptDTO data = new FeeReceiptDTO();
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.acayyearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.MI_ID = id;
            return feeTrailAuditreport.getinsdetils(data);
        }
        [HttpPost]
        [Route("getreport")]
        public FeeReceiptDTO getreport([FromBody] FeeReceiptDTO data123)
        {
         
            data123.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.acayyearid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
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
