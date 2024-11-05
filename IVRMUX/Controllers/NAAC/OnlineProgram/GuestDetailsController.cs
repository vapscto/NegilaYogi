using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.NAAC.OnlineProgram;

namespace IVRMUX.Controllers.OnlineProgram
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class GuestDetailsController : Controller
    {
        GuestDetailsDelegate oed = new GuestDetailsDelegate();
        [Route("getloaddata/{id:int}")]
        public OnlineProgramDTO getloaddata(int id)
        {
            OnlineProgramDTO data = new OnlineProgramDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.getloaddata(data);
        }


        [HttpPost]
        [Route("savedetail2")]
        public OnlineProgramDTO savedetail2([FromBody] OnlineProgramDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.savedetail(data);
        }



        [Route("getalldetailsviewrecords/{id:int}")]
        public OnlineProgramDTO getalldetailsviewrecords(int id)
        {
            OnlineProgramDTO data = new OnlineProgramDTO();
            data.PRYR_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.getalldetailsviewrecords(data);
        }


        [Route("getdetails/{id:int}")]
        public OnlineProgramDTO getdetails(int id)
        {
            OnlineProgramDTO data = new OnlineProgramDTO();
            data.PRYRG_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.getdetails(data);
        }


        [Route("deleterecord/{id:int}")]
        public OnlineProgramDTO deleterecord(int id)
        {
            OnlineProgramDTO data = new OnlineProgramDTO();
            data.PRYRG_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return oed.deleterecord(data);
        }
    }
}
