using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using PreadmissionDTOs.com.vaps.College.Fees;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Transactions;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Transactions
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CollegeFeeTransactionController : Controller
    {

        private readonly IMemoryCache _MemoryCache;
        CollegeFeeTransactionDelegate od = new CollegeFeeTransactionDelegate();
        // GET: api/values

        public CollegeFeeTransactionController(IMemoryCache memCache)
        {
            _MemoryCache = memCache;
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CollegeFeeTransactionDTO Get(int id)
        {
            CollegeFeeTransactionDTO data = new CollegeFeeTransactionDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            string acadyear= Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            data.ASMAY_Year = acadyear;
            //CollegeFeeTransactionDTO temp = SetGetMemoryCache(data);

            data.RoleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return od.getdata(data);

            //return temp;
        }

        private CollegeFeeTransactionDTO SetGetMemoryCache(CollegeFeeTransactionDTO data)
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
            CollegeFeeTransactionDTO Students;

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
                Students = _MemoryCache.Get(key) as CollegeFeeTransactionDTO;
                ViewBag.Status = "Data is Retrieved from in Cache";
            }
            return Students;
        }



        //[Route("getstudlistgroup/{id:int}")]
        //public CollegeFeeTransactionDTO Getstudacademicgrp(int id)
        //{
        //    return od.getdatastuacadgrp(id);
        //}

        // POST api/values
        [HttpPost]
        public CollegeFeeTransactionDTO savedata([FromBody] CollegeFeeTransactionDTO pgmodu)
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
            pgmodu.UserId= UserId;

            //pgmodu.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.savedetails(pgmodu);
        }

        [Route("getacademicyear")]
        public CollegeFeeTransactionDTO Getstudacademic([FromBody]CollegeFeeTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.UserId = UserId;

            return od.getdatastuacad(value);
        }

        [Route("dynamicfinecalculation")]
        public CollegeFeeTransactionDTO dynamicfinecalculation([FromBody]CollegeFeeTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.UserId = UserId;

            return od.dynamicfinecalculation(value);
        }

        
        [Route("getstudlistgroup")]
        public CollegeFeeTransactionDTO Getstudacademicgrp([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            data.ASMAY_Year= Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return od.getdatastuacadgrp(data);
        }


        [Route("printreceipt")]
        public CollegeFeeTransactionDTO printrec([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;
            return od.printrecdelegate(data);
        }

        [HttpPost]
        [Route("getgroupmappedheads")]
        public CollegeFeeTransactionDTO getstuddetails([FromBody]CollegeFeeTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.UserId = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.ASMAY_Id = ASMAY_Id;
           
            return od.getstuddet(value);
        }


        [Route("getgroupmappedheadsnew")]
        public CollegeFeeTransactionDTO getstuddetailsnew([FromBody]CollegeFeeTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.UserId = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //value.ASMAY_Id = ASMAY_Id;

            return od.getstuddetnew(value);
        }


        [Route("Deletedetails")]
        public CollegeFeeTransactionDTO deletereceipt([FromBody]CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return od.delrec(data);
        }



        [Route("feereceiptduplicate")]
        public CollegeFeeTransactionDTO duplicatereceipt([FromBody]CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return od.duplicaterec(data);
        }

        [HttpPost]
        [Route("get_grp_reptno")]
        public CollegeFeeTransactionDTO get_grp_reptno([FromBody] CollegeFeeTransactionDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_grp_reptno(categorypage);
        }

        [Route("searchfilter")]
        public CollegeFeeTransactionDTO searchfilter([FromBody]CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            return od.getsearchfilter(data);
        }

        [HttpPost]
        [Route("searching")]
        public CollegeFeeTransactionDTO searching([FromBody] CollegeFeeTransactionDTO categorypage)
        {
           // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.UserId = UserId;
            return od.searching(categorypage);
        }

        [Route("edittransaction")]
        public CollegeFeeTransactionDTO edittran([FromBody] CollegeFeeTransactionDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.edittrans(categorypage);
        }

        [Route("printreceiptnew")]
        public CollegeFeeTransactionDTO printrecnew([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.printrecdelegatenew(data);
        }


        [Route("Search_Chaln_No")]
        public CollegeFeeTransactionDTO Search_Chaln_No([FromBody] CollegeFeeTransactionDTO categorypage)
        {
            categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.Search_Chaln_No(categorypage);
        }
        [Route("Save_Chaln_No")]
        public CollegeFeeTransactionDTO Save_Chaln_No([FromBody] CollegeFeeTransactionDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.Save_Chaln_No(categorypage);
        }

        [Route("viewstatus")]
        public CollegeFeeTransactionDTO viewstatus([FromBody] CollegeFeeTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.viewstatus(data);
        }
    }
}
