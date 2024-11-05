using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeAmountEntryController : Controller
    {
        FeeAmountEntryDelegate od = new FeeAmountEntryDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeAmountEntryDTO Get(int id)
        {
            FeeAmountEntryDTO pgmodu = new FeeAmountEntryDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = ASMAY_Id;
            return od.getdata(pgmodu);
        }
        [Route("getalldetailsOnselectiontype")]
        public FeeAmountEntryDTO getalldetailsOnselectiontype([FromBody] FeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getalldetailsOnselectiontype(data);


        }


        [Route("Editdetails/{id:int}")]
        public FeeAmountEntryDTO EditDetails(int id)
        {
            FeeAmountEntryDTO pgmodu = new FeeAmountEntryDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = ASMAY_Id;
            pgmodu.FYGHM_Id = id;

            return od.EditDetails(pgmodu);
        }

        [HttpPost]

        [Route("paymentdetails")]
        public FeeAmountEntryDTO paymentdetailsfn([FromBody] FeeAmountEntryDTO id)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            id.ASMAY_Id = ASMAY_Id;


            return od.paymentdetailsfnc(id);
        }

        [Route("getgroupmappedheads")]
        public FeeAmountEntryDTO getgroupheaddetails([FromBody] FeeAmountEntryDTO pgmodu)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            //int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = pgmodu.ASMAY_Id;

            return od.getgroupheaddetails(pgmodu);
        }

      
        public FeeAmountEntryDTO savedata([FromBody] FeeAmountEntryDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails(pgmodu);
        }

        [HttpPost("{id}")]
        public FeeAmountEntryDTO Put(int id, [FromBody]FeeAmountEntryDTO value)
        {
            return od.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }
 
        [Route("Deletedetails")]
        public FeeAmountEntryDTO Delete([FromBody]FeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.deleterec(data);
        }

        [HttpPost]
        [Route("selectacademicyear")]
        public FeeAmountEntryDTO selectacademicyear([FromBody] FeeAmountEntryDTO pgmodu)
        {
            // FeeAmountEntryDTO data = new FeeAmountEntryDTO();
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return od.selectacade(pgmodu);
        }

    }
}
