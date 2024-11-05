using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeAdjustmentController : Controller
    {
        FeeAdjustmentDelegate od = new FeeAdjustmentDelegate();


        [HttpGet]//1
        [Route("getalldetails/{id:int}")]
        public FeeStudentAdjustmentDTO getinitialdropdowns(int id)
        {
            FeeStudentAdjustmentDTO data = new FeeStudentAdjustmentDTO();
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
        public FeeStudentAdjustmentDTO Getclass([FromBody]FeeStudentAdjustmentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdataclass(data);
        }
        // [HttpPost]//3
        [Route("getsection")]
        public FeeStudentAdjustmentDTO Getsection([FromBody]FeeStudentAdjustmentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdatasection(data);
        }
        // [HttpPost]//4
        [Route("getstudent")]
        public FeeStudentAdjustmentDTO GetStudent([FromBody]FeeStudentAdjustmentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getdatastudent(value);
        }
        // [HttpPost]//5
        [Route("getbothgroup")]
        public FeeStudentAdjustmentDTO Getbothgroup([FromBody]FeeStudentAdjustmentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getdatabothgroup(value);
        }
        //  [HttpPost]//6
        [Route("getfromhead")]
        public FeeStudentAdjustmentDTO Getfromhead([FromBody]FeeStudentAdjustmentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getdatafromhead(value);
        }
        //  [HttpPost]//7
        [Route("gettohead")]
        public FeeStudentAdjustmentDTO Gettohead([FromBody]FeeStudentAdjustmentDTO value)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            value.userid = UserId;
            return od.getdatatohead(value);
        }

        [HttpPost]//8
        [Route("savedata")]
        public FeeStudentAdjustmentDTO savedataa([FromBody] FeeStudentAdjustmentDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.savedatadelegate(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeStudentAdjustmentDTO deactvate([FromBody] FeeStudentAdjustmentDTO id)
        {
            return od.deactivate(id);
        }
        [Route("Editdetails/{id:int}")]//10
        public FeeStudentAdjustmentDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return od.EditDetails(id);
        }
        //for edit
        [Route("getdetails/{id:int}")]//11
        public FeeStudentAdjustmentDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
                                                                    // id = 12;
            return od.EditDetails(id);
        }
        [HttpDelete]
        [Route("Deletedetails/{id:int}")]
        public FeeStudentAdjustmentDTO Delete(int id)
        {
            return od.deleterec(id);
        }
        [HttpPost]
        [Route("searching")]
        public FeeStudentAdjustmentDTO searching([FromBody] FeeStudentAdjustmentDTO data)
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
