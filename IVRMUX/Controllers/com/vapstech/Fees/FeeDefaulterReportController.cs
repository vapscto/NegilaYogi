using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeDefaulterReportController : Controller
    {

        FeeDefaulterReportDelegate FDR = new FeeDefaulterReportDelegate();


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTransactionPaymentDTO getalldetails(int id)
        {
            //id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //return FDR.getdetails(id);
            FeeTransactionPaymentDTO data = new FeeTransactionPaymentDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return FDR.getdetails(data);
        }


        [HttpPost]
        [Route("getgrpterms/")]
        public FeeTransactionPaymentDTO getgrpterms([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.ASMAY_Id = ASMAY_Id;

            return FDR.getgrpterms(value);
        }

        [HttpPost]
        [Route("radiobtndata/")]
        public FeeTransactionPaymentDTO getradiodata([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            var data = FDR.getradiofiltereddata(value);

            if (data.FBD_Id == data.ASMAY_Id)
            {
                HttpContext.Session.SetInt32("FMT", Convert.ToInt32(data.FMT_ID));
            }

            return data;
        }


        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] FeeTransactionPaymentDTO MMD)
        {
            return FDR.ExportToExcle(MMD);
        }

        [Route("Print/")]
        public string Print([FromBody] FeeTransactionPaymentDTO MMD)
        {
            return FDR.ExportToExcle(MMD);
        }

        [HttpPost]
        [Route("getsection")]
        public FeeTransactionPaymentDTO getsection([FromBody]FeeTransactionPaymentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return FDR.getsection(data);
        }

        [Route("SendSms/")]
        public FeetransactionSMS SendSms([FromBody]FeetransactionSMS value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            value.FMT_ID = Convert.ToInt32(HttpContext.Session.GetInt32("FMT"));
            return FDR.sendsms(value);
        }
        [Route("Sendmail/")]
        public FeeTransactionPaymentDTO Sendmail([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return FDR.sendemail(value);
        }


        [Route("get_groups")]
        public FeeTransactionPaymentDTO get_groups([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            return FDR.get_groups(value);
        }

        //=========================================Staff Poratl
        [Route("getstaffwiseclass")]
        public FeeTransactionPaymentDTO getstaffwiseclass([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            return FDR.getstaffwiseclass(value);
        }

        [HttpPost]
        [Route("getStaffterms/")]
        public FeeTransactionPaymentDTO getStaffterms([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //value.ASMAY_Id = ASMAY_Id;

            return FDR.getStaffterms(value);
        }

        [Route("saveremark")]
        public FeeTransactionPaymentDTO saveremark([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;


            return FDR.saveremark(value);
        }
        [Route("feeremarkreport")]
        public FeeTransactionPaymentDTO feeremarkreport([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return FDR.feeremarkreport(value);
        }
        [Route("feeremarkload/{id:int}")]
        public FeeTransactionPaymentDTO feeremarkload(int id)
        {
            FeeTransactionPaymentDTO value = new FeeTransactionPaymentDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            value.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return FDR.feeremarkload(value);
        }
         [Route("feeremarksection")]
        public FeeTransactionPaymentDTO feeremarksection([FromBody]FeeTransactionPaymentDTO value)
        {
          
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            value.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return FDR.feeremarksection(value);
        }

    }
}
