using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeStudentTransactionController : Controller
    {

        private readonly IMemoryCache _MemoryCache;
        FeeStudentTransactionDelegate od = new FeeStudentTransactionDelegate();
        // GET: api/values

        public FeeStudentTransactionController(IMemoryCache memCache)
        {
            _MemoryCache = memCache;
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeStudentTransactionDTO Get(int id)
        {
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            string acadyear= Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            data.ASMAY_Year = acadyear;
            //FeeStudentTransactionDTO temp = SetGetMemoryCache(data);

            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return od.getdata(data);

            //return temp;
        }

        private FeeStudentTransactionDTO SetGetMemoryCache(FeeStudentTransactionDTO data)
        {
            //2
           
            //if (data.MI_Id == 4)
            //{
            string key = "Baldwin-Student-Cache-" + data.MI_Id.ToString();
            //}
            //else if(data.MI_Id == 5)
            //{
            //    key = "MyMemoryKey-Cache-5";
            //}
            //else if (data.MI_Id == 6)
            //{
            //    key = "MyMemoryKey-Cache-6";

            //}
            FeeStudentTransactionDTO Students;

            //3: We will try to get the Cache data
            //If the data is present in cache the 
            //Condition will be true else it is false 
            if (!_MemoryCache.TryGetValue(key, out Students))
            {
                //4.fetch the data from the object
                Students = od.getdata(data);
                //5.Save the received data in cache
                _MemoryCache.Set(key, Students,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                ViewBag.Status = "Data is added in Cache";

            }
            else
            {
                Students = _MemoryCache.Get(key) as FeeStudentTransactionDTO;
                ViewBag.Status = "Data is Retrieved from in Cache";
            }
            return Students;
        }



        //[Route("getstudlistgroup/{id:int}")]
        //public FeeStudentTransactionDTO Getstudacademicgrp(int id)
        //{
        //    return od.getdatastuacadgrp(id);
        //}

        // POST api/values
        [HttpPost]
        public FeeStudentTransactionDTO savedata([FromBody] FeeStudentTransactionDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            //pgmodu.MI_ID = 2;

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            pgmodu.userid= UserId;


            //pgmodu.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.savedetails(pgmodu);
        }

        [Route("getacademicyear")]
        public FeeStudentTransactionDTO Getstudacademic([FromBody]FeeStudentTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            return od.getdatastuacad(value);
        }

        [Route("getstudlistgroup")]
        public FeeStudentTransactionDTO Getstudacademicgrp([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.ASMAY_Year= Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return od.getdatastuacadgrp(data);
        }


        [Route("printreceipt")]
        public FeeStudentTransactionDTO printrec([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.printrecdelegate(data);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public FeeStudentTransactionDTO getstuddetails([FromBody]FeeStudentTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.ASMAY_Id = value.ASMAY_Id;
           
            return od.getstuddet(value);
        }


        [Route("getgroupmappedheadsnew")]
        public FeeStudentTransactionDTO getstuddetailsnew([FromBody]FeeStudentTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.ASMAY_Id = value.ASMAY_Id;

            return od.getstuddetnew(value);
        }


        [Route("Deletedetails")]
        public FeeStudentTransactionDTO deletereceipt([FromBody]FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            return od.delrec(data);
        }



        [Route("feereceiptduplicate")]
        public FeeStudentTransactionDTO duplicatereceipt([FromBody]FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            return od.duplicaterec(data);
        }

        [HttpPost]
        [Route("get_grp_reptno")]
        public FeeStudentTransactionDTO get_grp_reptno([FromBody] FeeStudentTransactionDTO categorypage)
        {
            //categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.ASMAY_Id = categorypage.ASMAY_Id;
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_grp_reptno(categorypage);
        }

        [Route("searchfilter")]
        public FeeStudentTransactionDTO searchfilter([FromBody]FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            return od.getsearchfilter(data);
        }

        [HttpPost]
        [Route("searching")]
        public FeeStudentTransactionDTO searching([FromBody] FeeStudentTransactionDTO categorypage)
        {
            //categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.ASMAY_Id = categorypage.ASMAY_Id;
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.userid = UserId;
            return od.searching(categorypage);
        }

        [Route("edittransaction")]
        public FeeStudentTransactionDTO edittran([FromBody] FeeStudentTransactionDTO categorypage)
        {
            //categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.ASMAY_Id = categorypage.ASMAY_Id;
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.edittrans(categorypage);
        }

        [Route("printreceiptnew")]
        public FeeStudentTransactionDTO printrecnew([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = data.ASMAY_Id;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.printrecdelegatenew(data);
        }


        [Route("Search_Chaln_No")]
        public FeeStudentTransactionDTO Search_Chaln_No([FromBody] FeeStudentTransactionDTO categorypage)
        {
            //categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.ASMAY_Id = categorypage.ASMAY_Id;
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.Search_Chaln_No(categorypage);
        }
        [Route("Save_Chaln_No")]
        public FeeStudentTransactionDTO Save_Chaln_No([FromBody] FeeStudentTransactionDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.Save_Chaln_No(categorypage);
        }

        [Route("SendEmail")]
        public FeeStudentTransactionDTO SendEmail([FromBody] FeeStudentTransactionDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.SendEmail(categorypage);
        }

        [Route("viewstatus")]
        public FeeStudentTransactionDTO viewstatus([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.viewstatus(data);
        }

        [Route("viewpaydetails")]
        public FeeStudentTransactionDTO viewpaydetails([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.viewpaydetails(data);
        }

        [Route("viewpayexcessdetails")]
        public FeeStudentTransactionDTO viewpayexcessdetails([FromBody] FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.viewpayexcessdetails(data);
        }


        [Route("Rebateapplyandsave")]
        public FeeStudentTransactionDTO Rebateapplyandsave([FromBody] FeeStudentTransactionDTO pgmodu)
        {


            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            pgmodu.userid = UserId;

            return od.Rebateapplyandsave(pgmodu);
        }
        [Route("rebateamountcalc")]
        public FeeStudentTransactionDTO rebateamountcalc([FromBody]FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.ASMAY_Id = data.ASMAY_Id;

            return od.rebateamountcalc(data);
        }


    }
}
