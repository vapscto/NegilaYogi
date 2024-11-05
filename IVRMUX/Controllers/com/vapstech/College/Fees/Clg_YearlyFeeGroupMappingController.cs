using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Masters
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class Clg_YearlyFeeGroupMappingController : Controller
    {
        // GET: api/values
        Clg_YearlyFeeGroupMappingDelegate od = new Clg_YearlyFeeGroupMappingDelegate();
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CLG_YearlyFeeGroupHeadMapping_DTO Get(int id)
        {
            CLG_YearlyFeeGroupHeadMapping_DTO pgmodu = new CLG_YearlyFeeGroupHeadMapping_DTO();
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return od.getdata(pgmodu);
        }

        [Route("getadetailsongroup")]
        public CLG_YearlyFeeGroupHeadMapping_DTO Getgrpdata([FromBody] CLG_YearlyFeeGroupHeadMapping_DTO pgmodu)
         {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return od.getdataongroup(pgmodu);
        }

        [Route("Editdetails/{id:int}")]
        public CLG_YearlyFeeGroupHeadMapping_DTO EditDetails(int id)
        {
            CLG_YearlyFeeGroupHeadMapping_DTO pgmodu = new CLG_YearlyFeeGroupHeadMapping_DTO();
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            pgmodu.FYGHM_Id = id;
            return od.EditDetails(pgmodu);
        }

        [HttpPost]
        public CLG_YearlyFeeGroupHeadMapping_DTO savedata([FromBody] CLG_YearlyFeeGroupHeadMapping_DTO pgmodu)
        {
            pgmodu.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            pgmodu.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedetails(pgmodu);
        }

        [HttpPost("{id}")]
        public CLG_YearlyFeeGroupHeadMapping_DTO Put(int id, [FromBody]CLG_YearlyFeeGroupHeadMapping_DTO value)
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
        public CLG_YearlyFeeGroupHeadMapping_DTO Delete(int id)
        {
            return od.deleterec(id);
        }

        [Route("selectacademic")]
        public CLG_YearlyFeeGroupHeadMapping_DTO getInitialData([FromBody] CLG_YearlyFeeGroupHeadMapping_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return od.selectacade(data);
        }
    }
}
