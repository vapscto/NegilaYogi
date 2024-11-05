using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class ExcelImportNotDoneReportController : Controller
    {

        ExcelImportNotDoneReportDelegate feeTrailAuditreport = new ExcelImportNotDoneReportDelegate();



        [Route("getalldetails123")]
        public ExcelImportNotDoneReportDTO Get123([FromBody] ExcelImportNotDoneReportDTO data)
        {
            //ExcelImportNotDoneReportDTO dt = new ExcelImportNotDoneReportDTO();
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
        public ExcelImportNotDoneReportDTO getsection([FromBody]ExcelImportNotDoneReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return feeTrailAuditreport.getsection(data);
        }

        [HttpPost]
        [Route("getstudent")]
        public ExcelImportNotDoneReportDTO getstudent([FromBody]ExcelImportNotDoneReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return feeTrailAuditreport.getstudent(data);
        }


        [HttpPost]
        [Route("getreport")]
        public ExcelImportNotDoneReportDTO getreport([FromBody] ExcelImportNotDoneReportDTO data123)
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
        public ExcelImportNotDoneReportDTO getstuddetails([FromBody]ExcelImportNotDoneReportDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //   value.asmay_id = ASMAY_Id;

            return feeTrailAuditreport.getstuddet(value);
        }


        [Route("get_groups")]
        public ExcelImportNotDoneReportDTO get_groups([FromBody]ExcelImportNotDoneReportDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            return feeTrailAuditreport.get_groups(value);
        }
        [HttpDelete]
        [Route("Deletedetails/{id:int}")]
        public ExcelImportNotDoneReportDTO Delete(int id)
        {
            return feeTrailAuditreport.deleterec(id);
        }
    }
}
