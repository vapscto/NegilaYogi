using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class CollegeStaffAndOtherTransactionController : Controller
    {
        private readonly IMemoryCache _MemoryCache;
        CollegeStaffAndOtherTransactionDelegate od = new CollegeStaffAndOtherTransactionDelegate();
        
        public CollegeStaffAndOtherTransactionController(IMemoryCache memCache)
        {
            _MemoryCache = memCache;
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CollegeStaffAndOtherTransactionDTO Get(int id)
        {
            CollegeStaffAndOtherTransactionDTO data = new CollegeStaffAndOtherTransactionDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            string acadyear = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            data.ASMAY_Year = acadyear;
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return od.getdata(data);
        }

        

        [HttpPost]
        [Route("feereceiptduplicate")]
        public CollegeStaffAndOtherTransactionDTO duplicatereceipt([FromBody]CollegeStaffAndOtherTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

          
            return od.duplicaterec(data);
        }


        [Route("get_grp_reptno")]
        public CollegeStaffAndOtherTransactionDTO get_grp_reptno([FromBody] CollegeStaffAndOtherTransactionDTO categorypage)
        {
          
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_grp_reptno(categorypage);
        }

        [Route("edittransactionstaff")]
        public CollegeStaffAndOtherTransactionDTO edittran([FromBody] CollegeStaffAndOtherTransactionDTO categorypage)
        {
        
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.edittrans(categorypage);
        }

     
        [Route("select_emp")]
        public CollegeStaffAndOtherTransactionDTO select_emp([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

          

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

        

            return od.select_emp(data);
        }
        [Route("select_student")]
        public CollegeStaffAndOtherTransactionDTO select_student([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

           
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            data.ASMAY_Year = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));

            return od.select_student(data);
        }
        [Route("getgroupmappedheadsnew_st")]
        public CollegeStaffAndOtherTransactionDTO getgroupmappedheadsnew_st([FromBody]CollegeStaffAndOtherTransactionDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            return od.getgroupmappedheadsnew_st(value);
        }
        [Route("savedata_st")]
        public CollegeStaffAndOtherTransactionDTO savedata_st([FromBody] CollegeStaffAndOtherTransactionDTO pgmodu)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            pgmodu.userid = UserId;

       
            return od.savedata_st(pgmodu);
        }
        [Route("searching_s")]
        public CollegeStaffAndOtherTransactionDTO searching_s([FromBody] CollegeStaffAndOtherTransactionDTO categorypage)
        {
       
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.userid = UserId;
            return od.searching_s(categorypage);
        }
        [Route("searching_o")]
        public CollegeStaffAndOtherTransactionDTO searching_o([FromBody] CollegeStaffAndOtherTransactionDTO categorypage)
        {
        
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            categorypage.userid = UserId;
            return od.searching_o(categorypage);
        }
        [Route("printreceipt_s")]
        public CollegeStaffAndOtherTransactionDTO printreceipt_s([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            return od.printreceipt_s(data);
        }
        [Route("printreceipt_o")]
        public CollegeStaffAndOtherTransactionDTO printreceipt_o([FromBody] CollegeStaffAndOtherTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.printreceipt_o(data);
        }
        [Route("Deletedetails_s")]
        public CollegeStaffAndOtherTransactionDTO deletereceipt_s([FromBody]CollegeStaffAndOtherTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.deletereceipt_s(data);
        }
        [Route("Deletedetails_o")]
        public CollegeStaffAndOtherTransactionDTO deletereceipt_o([FromBody]CollegeStaffAndOtherTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            

            return od.deletereceipt_o(data);
        }

        [Route("getacademicyear")]
        public CollegeStaffAndOtherTransactionDTO getacademicyear([FromBody]CollegeStaffAndOtherTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.getacademicyear(data);
        }

    }
}
