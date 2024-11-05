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
    public class FeeArrearRegisterReportController : Controller
    {

        FeeArrearRegisterReportDelegate feeTrailAuditreport = new FeeArrearRegisterReportDelegate();
       
     
      
        [Route("getalldetails123")]
        public FeeArrearRegisterReportDTO Get123([FromBody] FeeArrearRegisterReportDTO data)
        {
            //FeeArrearRegisterReportDTO dt = new FeeArrearRegisterReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmay_id = ASMAY_Id;

            return feeTrailAuditreport.getdata123(data);
        }

       

        [HttpPost]
        [Route("getsection")]
        public FeeArrearRegisterReportDTO getsection([FromBody]FeeArrearRegisterReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return feeTrailAuditreport.getsection(data);
        }

        [HttpPost]
        [Route("getstudent")]
        public FeeArrearRegisterReportDTO getstudent([FromBody]FeeArrearRegisterReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return feeTrailAuditreport.getstudent(data);
        }


        [HttpPost]
       [Route("getreport")]
        public FeeArrearRegisterReportDTO getreport([FromBody] FeeArrearRegisterReportDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;
            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data123.asmay_id = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data123.userid = UserId;
            return feeTrailAuditreport.getreport(data123);
        }



        [HttpPost]
        [Route("getgroupmappedheads")]
        public FeeArrearRegisterReportDTO getstuddetails([FromBody]FeeArrearRegisterReportDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;         
           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
         //   value.asmay_id = ASMAY_Id;

            return feeTrailAuditreport.getstuddet(value);
        }


        [Route("get_groups")]
        public FeeArrearRegisterReportDTO get_groups([FromBody]FeeArrearRegisterReportDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            return feeTrailAuditreport.get_groups(value);
        }

    }
}
