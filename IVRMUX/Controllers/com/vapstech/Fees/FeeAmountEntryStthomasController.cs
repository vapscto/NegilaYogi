using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class FeeAmountEntryStthomasController : Controller
    {
        FeeAmountEntryStthomasDelegate od = new FeeAmountEntryStthomasDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeAmountEntryStthomasDTO Get(int id)
        {
            FeeAmountEntryStthomasDTO pgmodu = new FeeAmountEntryStthomasDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = ASMAY_Id;
            return od.getdata(pgmodu);
        }
        [Route("getalldetailsOnselectiontype")]
        public FeeAmountEntryStthomasDTO getalldetailsOnselectiontype([FromBody] FeeAmountEntryStthomasDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getalldetailsOnselectiontype(data);


        }


        [Route("Editdetails/{id:int}")]
        public FeeAmountEntryStthomasDTO EditDetails(int id)
        {
            FeeAmountEntryStthomasDTO pgmodu = new FeeAmountEntryStthomasDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = ASMAY_Id;
            pgmodu.FYGHM_Id = id;

            return od.EditDetails(pgmodu);
        }

        [HttpPost]

        [Route("paymentdetails")]
        public FeeAmountEntryStthomasDTO paymentdetailsfn([FromBody] FeeAmountEntryStthomasDTO id)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            id.ASMAY_Id = ASMAY_Id;


            return od.paymentdetailsfnc(id);
        }

        [Route("getgroupmappedheads")]
        public FeeAmountEntryStthomasDTO getgroupheaddetails([FromBody] FeeAmountEntryStthomasDTO pgmodu)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = pgmodu.ASMAY_Id;

            return od.getgroupheaddetails(pgmodu);
        }


        public FeeAmountEntryStthomasDTO savedata([FromBody] FeeAmountEntryStthomasDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails(pgmodu);
        }

        [HttpPost("{id}")]
        public FeeAmountEntryStthomasDTO Put(int id, [FromBody]FeeAmountEntryStthomasDTO value)
        {
            return od.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        [Route("Deletedetails")]
        public FeeAmountEntryStthomasDTO Delete([FromBody]FeeAmountEntryStthomasDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.deleterec(data);
        }

        [HttpPost]
        [Route("selectacademicyear")]
        public FeeAmountEntryStthomasDTO selectacademicyear([FromBody] FeeAmountEntryStthomasDTO pgmodu)
        {
            FeeAmountEntryStthomasDTO data = new FeeAmountEntryStthomasDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return od.selectacade(pgmodu);
        }
    }
}
