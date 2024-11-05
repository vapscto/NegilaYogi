using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Caching.Memory;
using corewebapi18072016.Delegates.com.vapstech.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeePreadmissionController : Controller
    {
        private readonly IMemoryCache _MemoryCache;
        FeePreadmissionDelegate od = new FeePreadmissionDelegate();
        // GET: api/values

        public FeePreadmissionController(IMemoryCache memCache)
        {
            _MemoryCache = memCache;
        }

        [Route("getalldetails")]
        public FeeStudentTransactionDTO Get([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.getdata(data);
        }

        [Route("select_student")]
        public FeeStudentTransactionDTO selectstudent([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
         
            return od.selectstu(data);
        }

        [Route("getgroupmappedheadsnew_st")]
        public FeeStudentTransactionDTO selectgrptrm([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
          
            return od.selectgrptrm(data);
        }
        [Route("savedata_st")]
        public FeeStudentTransactionDTO savedata([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.savedata(data);
        }

        [Route("printreceipt_s")]
        public FeeStudentTransactionDTO printrec([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.printrec(data);
        }

        [Route("searchfilter")]
        public FeeStudentTransactionDTO searchstu([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.searchstu(data);
        }

        [Route("searching_s")]
        public FeeStudentTransactionDTO filtersearch([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.filtersearch(data);
        }


        [Route("get_grp_reptno")]
        public FeeStudentTransactionDTO receiptnogen([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.receiptnogen(data);
        }

        [Route("Deletedetails_s")]
        public FeeStudentTransactionDTO deleterece([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.deleterec(data);
        }


        [Route("filterstudent")]
        public FeeStudentTransactionDTO filstu([FromBody] FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            return od.filstu(data);
        }

    }
}





