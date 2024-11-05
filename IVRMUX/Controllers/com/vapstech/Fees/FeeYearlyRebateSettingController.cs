using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    public class FeeYearlyRebateSettingController : Controller
    {
        FeeYearlyRebateSettingDelegate feeHd = new FeeYearlyRebateSettingDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeYearlyRedateSettingDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return feeHd.getdetails(id);
        }
        //for edit
        [Route("getdetails/{id:int}")]
        public FeeYearlyRedateSettingDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return feeHd.getpagedetails(id);
        }
        [Route("Editdetails/{id:int}")]
        public FeeYearlyRedateSettingDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return feeHd.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public FeeYearlyRedateSettingDTO savedetail([FromBody] FeeYearlyRedateSettingDTO GroupHeadpage)
        {
            GroupHeadpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GroupHeadpage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeHd.savedetails(GroupHeadpage);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public FeeYearlyRedateSettingDTO Delete(int id)
        {
            return feeHd.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeYearlyRedateSettingDTO deactvate([FromBody] FeeYearlyRedateSettingDTO id)
        {
            return feeHd.deactivateAcademicYear(id);
        }

        [Route("validateordernumber")]
        public FeeYearlyRedateSettingDTO validateordernumber([FromBody] FeeYearlyRedateSettingDTO GroupOrder)
        {
            return feeHd.validateordernumber(GroupOrder);
        }
    }
}
