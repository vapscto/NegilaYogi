using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class CLGFeeAdjustmentController : Controller
    {
        CLGFeeAdjustmentDelegate od = new CLGFeeAdjustmentDelegate();


        [HttpGet]//1
        [Route("getalldetails/{id:int}")]
        public CLGFeeAdjustmentDTO getinitialdropdowns(int id)
        {
            CLGFeeAdjustmentDTO data = new CLGFeeAdjustmentDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdata(data);
        }

        // [HttpPost]//2
      
        [Route("getclass")]
        public CLGFeeAdjustmentDTO Getclass([FromBody]CLGFeeAdjustmentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdataclass(data);
        }
        // [HttpPost]//3
        [Route("getsection")]
        public CLGFeeAdjustmentDTO Getsection([FromBody]CLGFeeAdjustmentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdatasection(data);
        }
        // [HttpPost]//4
        [Route("getstudent")]
        public CLGFeeAdjustmentDTO GetStudent([FromBody]CLGFeeAdjustmentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getdatastudent(value);
        }
        // [HttpPost]//5
        [Route("getbothgroup")]
        public CLGFeeAdjustmentDTO Getbothgroup([FromBody]CLGFeeAdjustmentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getdatabothgroup(value);
        }
        //  [HttpPost]//6
        [Route("getfromhead")]
        public CLGFeeAdjustmentDTO Getfromhead([FromBody]CLGFeeAdjustmentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getdatafromhead(value);
        }
        //  [HttpPost]//7
        [Route("gettohead")]
        public CLGFeeAdjustmentDTO Gettohead([FromBody]CLGFeeAdjustmentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getdatatohead(value);
        }

        [HttpPost]//8
        [Route("savedata")]
        public CLGFeeAdjustmentDTO savedataa([FromBody] CLGFeeAdjustmentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.savedatadelegate(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public CLGFeeAdjustmentDTO deactvate([FromBody] CLGFeeAdjustmentDTO id)
        {
            return od.deactivate(id);
        }
        [Route("Editdetails/{id:int}")]//10
        public CLGFeeAdjustmentDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return od.EditDetails(id);
        }
        //for edit
        [Route("getdetails/{id:int}")]//11
        public CLGFeeAdjustmentDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
                                                                    // id = 12;
            return od.EditDetails(id);
        }
        [HttpDelete]
        [Route("Deletedetails/{id:int}")]
        public CLGFeeAdjustmentDTO Delete(int id)
        {
            return od.deleterec(id);
        }
        [HttpPost]
        [Route("searching")]
        public CLGFeeAdjustmentDTO searching([FromBody] CLGFeeAdjustmentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.searching(data);
        }
    }
}
