using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CLGFeeRefundableController : Controller
    {
        CLGFeeRefundableDelegate od = new CLGFeeRefundableDelegate();
        // GET: api/values

        [Route("getalldetails/{id:int}")]
        public CLGFeeRefundableDTO Get(int id)
        {

            CLGFeeRefundableDTO data = new CLGFeeRefundableDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdata(data);
        }

        [Route("getacademicyear/{id:int}")]
        public CLGFeeRefundableDTO Getstudacademic(int id)
        {
            CLGFeeRefundableDTO data = new CLGFeeRefundableDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdatastuacad(data);
        }

    
        [Route("GetStudentListByYear/{id:int}")]
        public CLGFeeRefundableDTO GetStudentListByYear(int id)
        {
            CLGFeeRefundableDTO frrr = new CLGFeeRefundableDTO();

            frrr.ASMAY_Id = id;
            frrr.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            frrr.userid = UserId;
            return od.getStudentdataByYear(frrr);
        }

        [Route("GetSection")]
        public CLGFeeRefundableDTO GetSection([FromBody] CLGFeeRefundableDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            yearclass.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.GetSection(yearclass);
        }
        [Route("get_semisters")]
        public CLGFeeRefundableDTO get_semisters([FromBody] CLGFeeRefundableDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            yearclass.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.get_semisters(yearclass);
        }
        [Route("GetStudent")]
        public CLGFeeRefundableDTO GetStudent([FromBody] CLGFeeRefundableDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            yearclass.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            yearclass.userid = UserId;
            return od.GetStudent(yearclass);
        }
        [Route("GetStudentListByamst")]
        public CLGFeeRefundableDTO GetStudentListByamst([FromBody] CLGFeeRefundableDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // yearclass.AMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            yearclass.userid = UserId;
            return od.GetStudentListByamst(yearclass);
        }


        //[Route("editdetails/{id:int}")]
        //public CLGFeeRefundableDTO editdetails(int id)
        //{
        //    CLGFeeRefundableDTO data = new CLGFeeRefundableDTO();
        //   //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //   // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

        //    data.FR_ID = id;

        //   // HttpContext.Session.SetString("refundid", id.ToString()); //Set
        //    return od.geteditdet(data);
        //}

        // POST api/values
        [HttpPost]
        public CLGFeeRefundableDTO savedata([FromBody] CLGFeeRefundableDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            //pgmodu.MI_ID = 2;

           // int chequebounce = 0;
           // if (HttpContext.Session.GetString("refundid") != null)
           // {
           //     chequebounce = Convert.ToInt32(HttpContext.Session.GetString("refundid"));//Get
           // }

          //  pgmodu.FR_ID = chequebounce;
          //  HttpContext.Session.Remove("refundid");
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            pgmodu.userid = UserId;
            // pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.savedetails(pgmodu);
        }

        [HttpPost]
        [Route("onselectgroup")]
        public CLGFeeRefundableDTO getstuddetails([FromBody]CLGFeeRefundableDTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getgroupheaddetails(value);
        }


        [HttpPost]
        [Route("modeofpayment")]
        public CLGFeeRefundableDTO getmodeofpaydata([FromBody]CLGFeeRefundableDTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getmodeofpaymentdata(value);
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("Deletedetails/{id:int}")]
        public CLGFeeRefundableDTO Delete(int id)
        {
            CLGFeeRefundableDTO data = new CLGFeeRefundableDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.FCR_Id = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.deleterec(data);
        }
        [HttpPost]
        [Route("searching")]
        public CLGFeeRefundableDTO searching([FromBody] CLGFeeRefundableDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.searching(data);
        }
    }
}
