using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeRefundableController : Controller
    {
        FeeRefundableDelegate od = new FeeRefundableDelegate();
        // GET: api/values

        [Route("getalldetails/{id:int}")]
        public FeeMasterRefundDTO Get(int id)
        {

            FeeMasterRefundDTO data = new FeeMasterRefundDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return od.getdata(data);
        }

        [Route("getacademicyear/{id:int}")]
        public FeeMasterRefundDTO Getstudacademic(int id)
        {
            FeeMasterRefundDTO data = new FeeMasterRefundDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdatastuacad(data);
        }

    
        [Route("GetStudentListByYear/{id:int}")]
        public FeeMasterRefundDTO GetStudentListByYear(int id)
        {
            FeeMasterRefundDTO frrr = new FeeMasterRefundDTO();

            frrr.ASMAY_ID = id;
            frrr.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            frrr.userid = UserId;
            return od.getStudentdataByYear(frrr);
        }

        [Route("GetSection")]
        public FeeMasterRefundDTO GetSection([FromBody] FeeMasterRefundDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            yearclass.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.GetSection(yearclass);
        }

        [Route("onselectacademicyear")]
        public FeeMasterRefundDTO onselectacademicyear([FromBody] FeeMasterRefundDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            yearclass.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.onselectacademicyear(yearclass);
        }
        [Route("GetStudent")]
        public FeeMasterRefundDTO GetStudent([FromBody] FeeMasterRefundDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            yearclass.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            yearclass.userid = UserId;
            return od.GetStudent(yearclass);
        }
        [Route("GetStudentListByamst")]
        public FeeMasterRefundDTO GetStudentListByamst([FromBody] FeeMasterRefundDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            // yearclass.AMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            yearclass.userid = UserId;
            return od.GetStudentListByamst(yearclass);
        }


        //[Route("editdetails/{id:int}")]
        //public FeeMasterRefundDTO editdetails(int id)
        //{
        //    FeeMasterRefundDTO data = new FeeMasterRefundDTO();
        //   //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //   // data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

        //    data.FR_ID = id;

        //   // HttpContext.Session.SetString("refundid", id.ToString()); //Set
        //    return od.geteditdet(data);
        //}

        // POST api/values
        [HttpPost]
        public FeeMasterRefundDTO savedata([FromBody] FeeMasterRefundDTO pgmodu)
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
            // pgmodu.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.savedetails(pgmodu);
        }

        [HttpPost]
        [Route("onselectgroup")]
        public FeeMasterRefundDTO getstuddetails([FromBody]FeeMasterRefundDTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //value.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getgroupheaddetails(value);
        }


        [HttpPost]
        [Route("modeofpayment")]
        public FeeMasterRefundDTO getmodeofpaydata([FromBody]FeeMasterRefundDTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
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
        public FeeMasterRefundDTO Delete(int id)
        {
            FeeMasterRefundDTO data = new FeeMasterRefundDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.FR_ID = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.deleterec(data);
        }
        [HttpPost]
        [Route("searching")]
        public FeeMasterRefundDTO searching([FromBody] FeeMasterRefundDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_ID = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.searching(data);
        }
        [Route("get_recepts")]
        public FeeMasterRefundDTO get_recepts([FromBody] FeeMasterRefundDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           return od.get_recepts(data);
        }
    }
}
