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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeWaivedOffController : Controller
    {
        FeeWaivedOffDelegate od = new FeeWaivedOffDelegate();


        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeStudentWaiveOffDTO getinitialdropdowns(int id)
        {
            FeeStudentWaiveOffDTO data = new FeeStudentWaiveOffDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdata(data);
        }
        // [HttpPost]
        [Route("getstudent")]
        public FeeStudentWaiveOffDTO GetStudent([FromBody]FeeStudentWaiveOffDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdatastudent(data);
        }
        // [HttpPost]
        [Route("getgroup")]
        public FeeStudentWaiveOffDTO Getgroup([FromBody]FeeStudentWaiveOffDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdatagroup(data);
        }
        // [HttpPost]
        [Route("gethead")]
        public FeeStudentWaiveOffDTO Gethead([FromBody]FeeStudentWaiveOffDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.getdatahead(data);
        }
        [HttpPost]
        [Route("savedata")]
        public FeeStudentWaiveOffDTO savedataa([FromBody] FeeStudentWaiveOffDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return od.savedatadelegate(data);
        }
      
        [Route("Editdetails/{id:int}")]
        public FeeStudentWaiveOffDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return od.EditDetails(id);
        }
        //for edit
        [Route("getdetails/{id:int}")]
        public FeeStudentWaiveOffDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); 
            return od.EditDetails(id);
        }
        [HttpDelete]
        [Route("Deletedetails/{id:int}")]
        public FeeStudentWaiveOffDTO Delete(int id)
        {
            return od.deleterec(id);
        }
        [HttpPost]
        [Route("searching")]
        public FeeStudentWaiveOffDTO searching([FromBody] FeeStudentWaiveOffDTO data)
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
