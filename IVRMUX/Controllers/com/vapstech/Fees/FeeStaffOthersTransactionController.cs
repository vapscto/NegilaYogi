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
    public class FeeStaffOthersTransactionController : Controller
    {

        private readonly IMemoryCache _MemoryCache;
        FeeStaffOthersTransactionDelegate od = new FeeStaffOthersTransactionDelegate();
        // GET: api/values

        public FeeStaffOthersTransactionController(IMemoryCache memCache)
        {
            _MemoryCache = memCache;
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeStaffOthersTransactionDTO Get(int id)
        {
            FeeStaffOthersTransactionDTO data = new FeeStaffOthersTransactionDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            string acadyear= Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            data.ASMAY_Year = acadyear;
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return od.getdata(data);
        }

        //private FeeStaffOthersTransactionDTO SetGetMemoryCache(FeeStaffOthersTransactionDTO data)
        //{
        //    string key = "Baldwin-Student-Cache-" + data.MI_Id.ToString();            
        //    FeeStaffOthersTransactionDTO Students;
        //    if (!_MemoryCache.TryGetValue(key, out Students))
        //    {
        //        Students = od.getdata(data);               
        //        _MemoryCache.Set(key, Students,
        //            new MemoryCacheEntryOptions()
        //            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

        //        ViewBag.Status = "Data is added in Cache";

        //    }
        //    else
        //    {
        //        Students = _MemoryCache.Get(key) as FeeStaffOthersTransactionDTO;
        //        ViewBag.Status = "Data is Retrieved from in Cache";
        //    }
        //    return Students;
        //}
        
        // POST api/values
        
        [HttpPost]
        [Route("feereceiptduplicate")]
        public FeeStaffOthersTransactionDTO duplicatereceipt([FromBody]FeeStaffOthersTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

          //  int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
          //  data.ASMAY_Id = ASMAY_Id;

            return od.duplicaterec(data);
        }

        
        [Route("get_grp_reptno")]
        public FeeStaffOthersTransactionDTO get_grp_reptno([FromBody] FeeStaffOthersTransactionDTO categorypage)
        {
           // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_grp_reptno(categorypage);
        }
        
        [Route("edittransactionstaff")]
        public FeeStaffOthersTransactionDTO edittran([FromBody] FeeStaffOthersTransactionDTO categorypage)
        {
          //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.edittrans(categorypage);
        }

        //for staff_others
        [Route("select_emp")]
        public FeeStaffOthersTransactionDTO select_emp([FromBody] FeeStaffOthersTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

          //  int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
          //  data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            //data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return od.select_emp(data);
        }
        [Route("select_student")]
        public FeeStaffOthersTransactionDTO select_student([FromBody] FeeStaffOthersTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

          //  int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           // data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return od.select_student(data);
        }
        [Route("getgroupmappedheadsnew_st")]
        public FeeStaffOthersTransactionDTO getgroupmappedheadsnew_st([FromBody]FeeStaffOthersTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

          //  int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
          //  value.ASMAY_Id = ASMAY_Id;

            return od.getgroupmappedheadsnew_st(value);
        }
        [Route("savedata_st")]
        public FeeStaffOthersTransactionDTO savedata_st([FromBody] FeeStaffOthersTransactionDTO pgmodu)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            pgmodu.userid = UserId;

            //pgmodu.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.savedata_st(pgmodu);
        }
        [Route("searching_s")]
        public FeeStaffOthersTransactionDTO searching_s([FromBody] FeeStaffOthersTransactionDTO categorypage)
        {
           // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.userid = UserId;
            return od.searching_s(categorypage);
        }
        [Route("searching_o")]
        public FeeStaffOthersTransactionDTO searching_o([FromBody] FeeStaffOthersTransactionDTO categorypage)
        {
            //categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.userid = UserId;
            return od.searching_o(categorypage);
        }
        [Route("printreceipt_s")]
        public FeeStaffOthersTransactionDTO printreceipt_s([FromBody] FeeStaffOthersTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            //data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return od.printreceipt_s(data);
        }
        [Route("printreceipt_o")]
        public FeeStaffOthersTransactionDTO printreceipt_o([FromBody] FeeStaffOthersTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.printreceipt_o(data);
        }
        [Route("Deletedetails_s")]
        public FeeStaffOthersTransactionDTO deletereceipt_s([FromBody]FeeStaffOthersTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
          //  data.ASMAY_Id = ASMAY_Id;

            return od.deletereceipt_s(data);
        }
        [Route("Deletedetails_o")]
        public FeeStaffOthersTransactionDTO deletereceipt_o([FromBody]FeeStaffOthersTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

           // int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           // data.ASMAY_Id = ASMAY_Id;

            return od.deletereceipt_o(data);
        }

        [Route("getacademicyear")]
        public FeeStaffOthersTransactionDTO getacademicyear([FromBody]FeeStaffOthersTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.getacademicyear(data);
        }

    }
}
