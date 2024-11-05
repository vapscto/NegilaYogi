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
    public class CollegeStudentwiseAmtEntryController : Controller
    {
        CollegeStudentwiseAmtEntryDelegate od = new CollegeStudentwiseAmtEntryDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        [Route("getalldetails")]
        public CLGFeeAmountEntryDTO Get([FromBody]CLGFeeAmountEntryDTO data)
        {
            CLGFeeAmountEntryDTO pgmodu = new CLGFeeAmountEntryDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.ASMAY_Id = ASMAY_Id;
            return od.getdata(pgmodu);
        }

        [Route("Deletedetails")]
        public CLGFeeAmountEntryDTO Deletedetails([FromBody]CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.deleterec(data);
        }


        [Route("selectacademicyear")]
        public CLGFeeAmountEntryDTO selectacademicyear([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.selectacademicyear(data);
        }




        [Route("selectcourse")]
        public CLGFeeAmountEntryDTO selectcou([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.getbran(data);
        }


        [Route("selectbran")]
        public CLGFeeAmountEntryDTO selectcoubran([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.getcoubransem(data);
        }

        [Route("getgroupmappedheads")]
        public CLGFeeAmountEntryDTO getgroupmappedhead([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.getgroupmapped(data);
        }


        [Route("selectsem")]
        public CLGFeeAmountEntryDTO selectsem([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.selectsem(data);
        }

        [Route("fillslabdetails")]
        public CLGFeeAmountEntryDTO fillslabdeta([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.fillslabde(data);
        }

        [Route("savedata")]
        public CLGFeeAmountEntryDTO saveda([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.savedataa(data);
        }

        [Route("getalldetailsOnselectiontype")]
        public CLGFeeAmountEntryDTO getalldetailsOnselectiontype([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getalldetailsOnselectiontype(data);


        }

        // Holy-Cross 14-03-2024

        [Route("getmappedconcessionheads")]
        public CLGFeeAmountEntryDTO getmappedconcessionheads([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return od.getmappedconcessionheads(data);
        }


        [Route("savescholershpheaddata")]
        public CLGFeeAmountEntryDTO savescholershpheaddata([FromBody] CLGFeeAmountEntryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));


            return od.savescholershpheaddata(data);
        }


    }
}
