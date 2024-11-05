using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeChequeBounceController : Controller
    {
        FeeChequeBounceDelegate od = new FeeChequeBounceDelegate();
        // GET: api/values

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeChequeBounceDTO Get(int id)
        {
            FeeChequeBounceDTO data = new FeeChequeBounceDTO();
            data.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.getdata(data);
        }

        [Route("getacademicyear/{id:int}")]
        public FeeChequeBounceDTO Getstudacademic(int id)
        {
            FeeChequeBounceDTO data = new FeeChequeBounceDTO();
            data.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.getdatastuacad(data);
        }

        [Route("getstudlistgroup/{id:int}")]
        public FeeChequeBounceDTO Getstudacademicgrp(int id)
        {
            FeeChequeBounceDTO data = new FeeChequeBounceDTO();
            data.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.getdatastuacadgrp(data);
        }

        [Route("getSchoolTypedetails/{id:int}")]
        public FeeChequeBounceDTO editdetails(int id)
        {
            FeeChequeBounceDTO data = new FeeChequeBounceDTO();
            data.FCB_Id = id;
            HttpContext.Session.SetString("chequebounceid", id.ToString()); //Set
            data.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.geteditdet(data);
        }

        // POST api/values
        [HttpPost]
        public FeeChequeBounceDTO savedata([FromBody] FeeChequeBounceDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            //pgmodu.MI_ID = 2;
            pgmodu.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            pgmodu.user_id = UserId;
            int chequebounce = 0;
            if (HttpContext.Session.GetString("chequebounceid") != null)
            {
                chequebounce = Convert.ToInt32(HttpContext.Session.GetString("chequebounceid"));//Get
            }

            pgmodu.FCB_Id = chequebounce;
            HttpContext.Session.Remove("chequebounceid");

            pgmodu.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.savedetails(pgmodu);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public FeeChequeBounceDTO getstuddetails([FromBody]FeeChequeBounceDTO value)
        {
            value.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.user_id = UserId;
            return od.getstuddet(value);
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("Deletedetails/{id:int}")]
        public FeeChequeBounceDTO Delete(int id)
        {
            FeeChequeBounceDTO data = new FeeChequeBounceDTO();
            data.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            data.FCB_Id = id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.deleterec(data);
        }
        [HttpPost]
        [Route("searching")]
        public FeeChequeBounceDTO searching([FromBody] FeeChequeBounceDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.searching(data);
        }
        [Route("get_students")]
        public FeeChequeBounceDTO get_students([FromBody] FeeChequeBounceDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.ASMAY_ID = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.get_students(data);
        }
        [Route("get_section")]
        public FeeChequeBounceDTO get_section([FromBody] FeeChequeBounceDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.ASMAY_ID = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.get_section(data);
        }
        [Route("get_receipts")]
        public FeeChequeBounceDTO get_receipts([FromBody] FeeChequeBounceDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.ASMAY_ID = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;
            return od.get_receipts(data);
        }
    }
}
