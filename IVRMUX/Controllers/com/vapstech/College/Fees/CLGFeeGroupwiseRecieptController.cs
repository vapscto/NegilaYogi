using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.College.Fees;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class CLGFeeGroupwiseRecieptController : Controller
    {
        CLGFeeGroupwiseRecieptDelegate od = new CLGFeeGroupwiseRecieptDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLGFeeGroupwiseRecieptDTO Get(int id)
        {
            CLGFeeGroupwiseRecieptDTO pgmodu = new CLGFeeGroupwiseRecieptDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = ASMAY_Id;
            return od.getdata(pgmodu);
        }
       
        [Route("selectacademicyear")]
        public CLGFeeGroupwiseRecieptDTO selectaca([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
         
            return od.getcourse(data);
        }

        [Route("onsemselection")]
        public CLGFeeGroupwiseRecieptDTO onsemselection([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           

            return od.onsemselection(data);
        }
        [Route("onselectsec")]
        public CLGFeeGroupwiseRecieptDTO onselectsec([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));


            return od.onselectsec(data);
        }
        

        [Route("selectcourse")]
        public CLGFeeGroupwiseRecieptDTO selectcou([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.getbran(data);
        }


        [Route("selectbran")]
        public CLGFeeGroupwiseRecieptDTO selectcoubran([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));


            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.getcoubransem(data);
        }

       



       

        [Route("getreceiptreport")]
        public CLGFeeGroupwiseRecieptDTO getreceiptreport([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.getreceiptreport(data);
        }
        

        [Route("getreceipt")]
        public CLGFeeGroupwiseRecieptDTO getreceipt([FromBody] CLGFeeGroupwiseRecieptDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          //  data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.getreceipt(data);
        }

    }
}
