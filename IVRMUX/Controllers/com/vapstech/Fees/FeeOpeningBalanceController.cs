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
    public class FeeOpeningBalanceController : Controller
    {

        FeeOpeningBalanceDelegate feeTrailAuditreport = new FeeOpeningBalanceDelegate();
       
     
       
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public FeeOpeningBalanceDTO Get123(int id)
        {
            FeeOpeningBalanceDTO data = new FeeOpeningBalanceDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmay_id = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return feeTrailAuditreport.getdata123(data);
        }


        [HttpPost]
        [Route("onselectacademicyear")]
        public FeeOpeningBalanceDTO onselectacademicyear([FromBody] FeeOpeningBalanceDTO data123)
        {
            data123.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.onselectacademicyear(data123);
        }


        //  POST api/values

        [HttpPost]
        [Route("getreport")]
        public FeeOpeningBalanceDTO getreport([FromBody]FeeOpeningBalanceDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            
            data123.MI_ID = mid;
            data123.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.getreport(data123);
        }
        [HttpPost]
        [Route("getclshead")]
        public FeeOpeningBalanceDTO getclshead([FromBody] FeeOpeningBalanceDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;
            data123.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.getclshead(data123);
        }
        [HttpPost]
        [Route("getgroup")]
        public FeeOpeningBalanceDTO getgroup([FromBody] FeeOpeningBalanceDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;
            data123.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.getgroup(data123);
        }
        [HttpPost]
        [Route("gethead")]
        public FeeOpeningBalanceDTO gethead([FromBody] FeeOpeningBalanceDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;
            data123.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.gethead(data123);
        }

        [HttpPost]
        [Route("filterstudents")]
        public FeeOpeningBalanceDTO filterstudents([FromBody]FeeOpeningBalanceDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.asmay_id = ASMAY_Id;
            value.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.filterstudents(value);
        }

        [HttpPost]
        [Route("savedata")]
        public FeeOpeningBalanceDTO getclassstudentlist([FromBody]FeeOpeningBalanceDTO student)
        {
            student.MI_ID= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            student.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));         
            return feeTrailAuditreport.getlisttwo(student);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public FeeOpeningBalanceDTO getstuddetails([FromBody]FeeOpeningBalanceDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;         
            value.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.getstuddet(value);
        }
        [HttpPost]
        [Route("getrefund")]
        public FeeOpeningBalanceDTO getrefund([FromBody]FeeOpeningBalanceDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //value.asmay_id = ASMAY_Id;
            value.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeTrailAuditreport.getrefund(value);
        }
        [HttpPost]
        [Route("DeleteEntry")]
        public FeeOpeningBalanceDTO DeleteEntry([FromBody] FeeOpeningBalanceDTO data)
        {
            return feeTrailAuditreport.DeletedEntry(data);

        }
        [HttpPost]
        [Route("searching")]
        public FeeOpeningBalanceDTO searching([FromBody] FeeOpeningBalanceDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmay_id = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return feeTrailAuditreport.searching(data);
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
