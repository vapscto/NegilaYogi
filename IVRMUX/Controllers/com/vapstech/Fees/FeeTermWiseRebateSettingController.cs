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
    public class FeeTermWiseRebateSettingController : Controller
    {
        FeeTermWiseRebateSettingDelegate feeHd = new FeeTermWiseRebateSettingDelegate();
      
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTermWiseRebateSettingDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return feeHd.getdetails(id);
        }
        //for edit

        [Route("getdetails/{id:int}")]
        public FeeTermWiseRebateSettingDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return feeHd.getpagedetails(id);
        }

        [HttpPost]
        public FeeTermWiseRebateSettingDTO savedetail([FromBody] FeeTermWiseRebateSettingDTO GroupHeadpage)
        {
            GroupHeadpage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GroupHeadpage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return feeHd.savedetails(GroupHeadpage);
        }

      
        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public FeeTermWiseRebateSettingDTO Delete(int id)
        {
            return feeHd.deleterec(id);
        }

        [HttpPost]
        [Route("deactivate")]
        public FeeTermWiseRebateSettingDTO deactvate([FromBody] FeeTermWiseRebateSettingDTO id)
        {
            return feeHd.deactivateAcademicYear(id);
        }

    }
}
