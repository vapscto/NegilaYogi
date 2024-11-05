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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class YearlyFeeGroupMappingController : Controller
    {
        YearlyFeeGroupMappingDelegate od = new YearlyFeeGroupMappingDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeYearlygroupHeadMappingDTO Get(int id)
        {
            FeeYearlygroupHeadMappingDTO pgmodu = new FeeYearlygroupHeadMappingDTO();
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getdata(pgmodu);
        }

        [Route("getadetailsongroup")]
        public FeeYearlygroupHeadMappingDTO Getgrpdata([FromBody] FeeYearlygroupHeadMappingDTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return od.getdataongroup(pgmodu);
        }

        [Route("Editdetails/{id:int}")]
        public FeeYearlygroupHeadMappingDTO EditDetails(int id)
        {
            FeeYearlygroupHeadMappingDTO pgmodu = new FeeYearlygroupHeadMappingDTO();
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.FYGHM_Id = id;
            return od.EditDetails(pgmodu);
        }

        [HttpPost]
        public FeeYearlygroupHeadMappingDTO savedata([FromBody] FeeYearlygroupHeadMappingDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            //pgmodu.MI_Id = 2;
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            pgmodu.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return od.savedetails(pgmodu);
        }

        [HttpPost("{id}")]
        public FeeYearlygroupHeadMappingDTO Put(int id, [FromBody]FeeYearlygroupHeadMappingDTO value)
        {
            value.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        [HttpDelete]
        [Route("Deletedetails/{id:int}")]
        public FeeYearlygroupHeadMappingDTO Delete(int id)
        {
            return od.deleterec(id);
        }

        [Route("selectacademic")]
        public FeeYearlygroupHeadMappingDTO getInitialData([FromBody] FeeYearlygroupHeadMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return od.selectacade(data);
        }

    }
}
