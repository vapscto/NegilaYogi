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
    public class FeeChallanReportController : Controller
    {

        FeeChallanReportDelegate feeTrailAuditreport = new FeeChallanReportDelegate();
       
     
       
        [HttpGet]
        [Route("getalldetails123/{id:int}")]
        public FeeChallanReportDTO Get123(int id)
        {
            FeeChallanReportDTO data = new FeeChallanReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.asmay_id = ASMAY_Id;

            return feeTrailAuditreport.getdata123(data);
        }
       
        //  POST api/values

        [HttpPost]
       [Route("getreport")]
        public FeeChallanReportDTO getreport([FromBody] FeeChallanReportDTO data123)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data123.MI_ID = mid;
            return feeTrailAuditreport.getreport(data123);
        }

        //Date: 27-10-2017 By Sripad Joshi.
        // Hutching Challan generation code starts here.
        [Route("getInstallments/{id:int}")]
        public FeeChallanReportDTO getInstallments(int id)
        {
            FeeChallanReportDTO dt = new FeeChallanReportDTO();
            dt.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dt.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dt.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dt.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (dt.Amst_Id == 0)
            {
                dt.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
            return feeTrailAuditreport.getinstallment(dt);
        }
        [Route("generateChallan")]
        public FeeChallanReportDTO generateHutchingChallan([FromBody] FeeChallanReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (data.Amst_Id==0)
            {
                data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
            return feeTrailAuditreport.generateChallan(data);
        }
        [Route("checkforchallan")]
        public FeeChallanReportDTO checkforchallan([FromBody] FeeChallanReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.FMT_Id = data.FMT_Id;
            if (data.Amst_Id == 0)
            {
                data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
            return feeTrailAuditreport.checkforchallan(data);
        }

        [Route("getChallandetails")]
        public FeeChallanReportDTO getChallandetails([FromBody] FeeChallanReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           // data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (data.Amst_Id == 0)
            {
                data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
            return feeTrailAuditreport.getChallandetails(data);

        }
        

       [Route("Deletedetails")]
        public FeeChallanReportDTO deletereceipt([FromBody]FeeChallanReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (data.Amst_Id == 0)
            {
                data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
            return feeTrailAuditreport.delrec(data);

          
        }

        [Route("getacademicyear")]
        public FeeChallanReportDTO getacademicyear([FromBody]FeeChallanReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            if (data.Amst_Id == 0)
            {
                data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
            return feeTrailAuditreport.getinstallment(data);
        }


        [Route("getstudlistgroup")]
        public FeeChallanReportDTO getstudlistgroup([FromBody]FeeChallanReportDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (data.Amst_Id == 0)
            {
                data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
            return feeTrailAuditreport.getstudlistgroup(data);
        }

        [Route("searching")]
        public FeeStudentTransactionDTO searching([FromBody]FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (data.Amst_Id == 0)
            {
                data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            }
            return feeTrailAuditreport.searching(data);


        }
    }
}
