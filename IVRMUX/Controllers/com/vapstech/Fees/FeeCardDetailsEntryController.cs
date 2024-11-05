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
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeCardDetailsEntryController : Controller
    {

        private readonly IMemoryCache _MemoryCache;
        FeeCardDetailsEntryDelegate od = new FeeCardDetailsEntryDelegate();
        // GET: api/values

        public FeeCardDetailsEntryController(IMemoryCache memCache)
        {
            _MemoryCache = memCache;
        }

        [HttpGet]
        [Route("getdata/{id:int}")]
        public FeeCardDetailEntryDTO getdata(int id)
        {
            FeeCardDetailEntryDTO data = new FeeCardDetailEntryDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            //int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.userid = UserId;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            string acadyear= Convert.ToString(HttpContext.Session.GetString("ASMAY_Year"));  
            return od.getdata(data);
        }

        [Route("searchfilter")]
        public FeeCardDetailEntryDTO searchfilter([FromBody]FeeCardDetailEntryDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return od.getsearchfilter(data);
        }

        [Route("getstudlistgroup")]
        public FeeCardDetailEntryDTO getstudlistgroup([FromBody] FeeCardDetailEntryDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;           

            return od.getstudlistgroup(data);
        }


        [Route("getgroupmappedheads")]
        public FeeCardDetailEntryDTO getgroupmappedheads([FromBody]FeeCardDetailEntryDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.ASMAY_Id = ASMAY_Id;

            return od.getgroupmappedheads(value);
        }

        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public FeeCardDetailEntryDTO savedata([FromBody] FeeCardDetailEntryDTO data)
        {
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.savedata(data);
        }
        [Route("editdetails/{id:int}")]
        public FeeCardDetailEntryDTO getdetail(int id)
        {
            return od.editdetails(id);
        }
        [Route("Deletedetails/{id:int}")]
        public FeeCardDetailEntryDTO Deletedetails(int id)
        {
            return od.Deletedetails(id);
        }
    }
}
