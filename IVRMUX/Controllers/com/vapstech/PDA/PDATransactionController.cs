using corewebapi18072016.Delegates.com.vapstech.PDA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.PDA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Controllers.com.vapstech.PDA
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PDATransactionController : Controller
    {

        PDATransactionDelegate pda = new PDATransactionDelegate();
        PDATransactionDTO data = new PDATransactionDTO();


        private readonly IMemoryCache _MemoryCache;

        // GET: api/values

        public PDATransactionController(IMemoryCache memCache)
        {
            _MemoryCache = memCache;
        }


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public PDATransactionDTO getalldetails(int id)
        {

            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            string acadyear = Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));
            data.ASMAY_Year = acadyear;
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return pda.getdetails(data);
        }

        [Route("searchfilter")]
        public FeeStudentTransactionDTO searchfilter([FromBody]FeeStudentTransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return pda.getsearchfilter(data);
        }


        [Route("Savedata")]
        public PDATransactionDTO Savedata([FromBody]PDATransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;

            return pda.Savedata(data);
        }

        [Route("searching")]
        public PDATransactionDTO searching([FromBody]PDATransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.user_id = UserId;

            return pda.searching(data);
        }

        [Route("Deletedetails")]
        public PDATransactionDTO Deletedetails([FromBody]PDATransactionDTO data)
        {

            return pda.Deletedetails(data);
        }



        [Route("getsection")]
        public PDATransactionDTO getsection([FromBody]PDATransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return pda.getsection(data);
        }


        [Route("getstudent")]
        public PDATransactionDTO getstudent([FromBody]PDATransactionDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_ID = mid;
            return pda.getstudent(data);
        }

    }

}
