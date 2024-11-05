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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CollegeFeePreadmissionTransaction : Controller
    {
        private readonly IMemoryCache _MemoryCache;
        CollegeFeePreadmissionTransactionDelegate od = new CollegeFeePreadmissionTransactionDelegate();
        // GET: api/values

        public CollegeFeePreadmissionTransaction(IMemoryCache memCache)
        {
            _MemoryCache = memCache;
        }

        [Route("getalldetails")]
        public CollegeFeeTransactionDTO Get([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.getdata(data);
        }

        [Route("select_student")]
        public CollegeFeeTransactionDTO selectstudent([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.selectstu(data);
        }

        [Route("getgroupmappedheadsnew_st")]
        public CollegeFeeTransactionDTO selectgrptrm([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.selectgrptrm(data);
        }
        [Route("savedata_st")]
        public CollegeFeeTransactionDTO savedata([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.savedata(data);
        }

        [Route("printreceipt_s")]
        public CollegeFeeTransactionDTO printrec([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.printrec(data);
        }

        [Route("searchfilter")]
        public CollegeFeeTransactionDTO searchstu([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.searchstu(data);
        }

        [Route("searching_s")]
        public CollegeFeeTransactionDTO filtersearch([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.filtersearch(data);
        }


        [Route("get_grp_reptno")]
        public CollegeFeeTransactionDTO receiptnogen([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.receiptnogen(data);
        }

        [Route("Deletedetails_s")]
        public CollegeFeeTransactionDTO deleterece([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.deleterec(data);
        }


        [Route("filterstudent")]
        public CollegeFeeTransactionDTO filstu([FromBody] CollegeFeeTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.UserId = UserId;

            return od.filstu(data);
        }
    }
}
