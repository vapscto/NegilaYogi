using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Fees;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class MonthlyCollectionReportController : Controller
    {
        MonthlyCollectionReportDelegate MCR = new MonthlyCollectionReportDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


      
        [Route("getalldetails")]
        public MonthlyCollectionReportDTO getalldetails([FromBody] MonthlyCollectionReportDTO data)
        {
           
            data.mid= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return MCR.getdetails(data);
        }


        [Route("getnameregno")]
        public MonthlyCollectionReportDTO getstuddetails([FromBody]MonthlyCollectionReportDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.mid = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            value.ASMAY_Id = ASMAY_Id;

            return MCR.getstuddet(value);
        }


        [Route("getreport")]
        public MonthlyCollectionReportDTO getreport([FromBody] MonthlyCollectionReportDTO mmd)
        {
            mmd.mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mmd.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            mmd.userid = UserId;
            return MCR.getreport(mmd);
        }

        [Route("get_groups")]
        public FeeTransactionPaymentDTO get_groups([FromBody]FeeTransactionPaymentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_ID = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            return MCR.get_groups(value);
        }
    }
}
